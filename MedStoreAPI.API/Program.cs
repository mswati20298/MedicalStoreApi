using System.Text;
using System.Threading.RateLimiting;
using MedStoreAPI.Common;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;
using MedStoreAPI.Infrastructure.Repositories;
using MedStoreAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
// Author: Mahesh Kumar
// Date: 26/07/2026
// Description: Application startup - registers DI services,
// CORS (for Angular frontend on localhost:4200), and Swagger.
// As new modules (Medicines, Batches, Invoices, etc.) are added,
// register their repositories/services here following the same pattern.
// -----------------------------------------------------

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI (with JWT "Authorize" button support)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// CORS - allow Angular dev server to call this API
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// -----------------------------------------------------
// JWT Authentication - validates tokens issued by IJwtTokenGenerator on login.
// -----------------------------------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// -----------------------------------------------------
// Common / Infrastructure services (used by every module)
// -----------------------------------------------------
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// -----------------------------------------------------
// Module-specific Repositories (Infrastructure) + Services (business logic).
// Pattern to follow for every new module (e.g. Batches, Invoices, Suppliers):
//   builder.Services.AddScoped<I{Entity}Repository, {Entity}Repository>();
//   builder.Services.AddScoped<I{Entity}Service, {Entity}Service>();
// -----------------------------------------------------
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ICustomersService, CustomersService>();

builder.Services.AddScoped<IMedicinesRepository, MedicinesRepository>();
builder.Services.AddScoped<IMedicinesService, MedicinesService>();

builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();
builder.Services.AddScoped<ISuppliersService, SuppliersService>();

builder.Services.AddScoped<IBatchesRepository, BatchesRepository>();
builder.Services.AddScoped<IBatchesService, BatchesService>();

builder.Services.AddScoped<IInvoicesRepository, InvoicesRepository>();
builder.Services.AddScoped<IInvoicesService, InvoicesService>();

builder.Services.AddScoped<ICustomerCreditsRepository, CustomerCreditsRepository>();
builder.Services.AddScoped<ICustomerCreditsService, CustomerCreditsService>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IStoresRepository, StoresRepository>();
builder.Services.AddScoped<IStoresService, StoresService>();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddScoped<IUnitsRepository, UnitsRepository>();
builder.Services.AddScoped<IUnitsService, UnitsService>();

builder.Services.AddScoped<IGSTSlabsRepository, GSTSlabsRepository>();
builder.Services.AddScoped<IGSTSlabsService, GSTSlabsService>();

builder.Services.AddScoped<IPaymentModesRepository, PaymentModesRepository>();
builder.Services.AddScoped<IPaymentModesService, PaymentModesService>();

// -----------------------------------------------------
// Author: Mahesh Kumar
// Date: 01/08/2026
// Description: Rate limiting for authentication endpoints (login, register,
// forgot-password). Built into ASP.NET Core (no extra NuGet package needed).
// Keyed per-client-IP so one abusive IP can't lock out/spam-brute-force the
// login or forgot-password flow for everyone else. Applied via
// [EnableRateLimiting("AuthPolicy")] on AuthController specifically - not
// applied globally, so normal app usage (Billing, Medicines, etc.) is
// completely unaffected.
// -----------------------------------------------------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 5, // max 5 login/register/forgot-password attempts per IP per minute
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        var response = ApiResponse<object>.Fail("Too many attempts. Please wait a minute and try again.");
        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
    };
});

var app = builder.Build();

// -----------------------------------------------------
// Author: Mahesh Kumar
// Date: 01/08/2026
// Description: Global exception handling. In Development we keep the
// detailed developer exception page (useful while building). In every
// other environment, ANY unhandled exception is caught here, logged
// server-side (never shown to the client), and converted into the same
// ApiResponse<T> shape every other endpoint already returns - so the
// frontend never has to special-case a raw 500 HTML/stack-trace response.
// -----------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (exceptionFeature is not null)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(exceptionFeature.Error, "Unhandled exception on {Path}", context.Request.Path);
            }

            var response = ApiResponse<object>.Fail("Something went wrong on our end. Please try again in a moment.");
            await context.Response.WriteAsJsonAsync(response);
        });
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // serves wwwroot/uploads/logos/* so LogoUrl paths are directly accessible
app.UseCors("AllowAngularApp");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
