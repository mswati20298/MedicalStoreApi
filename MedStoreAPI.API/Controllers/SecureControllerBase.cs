using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Base class for all controllers that deal with store-scoped
    /// data. Requires a valid JWT ([Authorize]) and exposes CurrentStoreID,
    /// read directly from the token's "StoreID" claim - NEVER from a
    /// client-supplied query/body value. This prevents a logged-in user of
    /// Store A from reading/editing Store B's data by simply changing a
    /// storeID parameter in the request.
    /// All module controllers (Customers, Medicines, Batches, Invoices, etc.)
    /// should inherit from this instead of ControllerBase directly.
    /// AuthController is the only exception (stays on ControllerBase +
    /// [AllowAnonymous] since no token exists yet at login/register time).
    /// </summary>
    [Authorize]
    public abstract class SecureControllerBase : ControllerBase
    {
        protected int CurrentStoreID
        {
            get
            {
                var claim = User.FindFirst("StoreID")
                    ?? throw new UnauthorizedAccessException("StoreID claim missing from token.");
                return int.Parse(claim.Value);
            }
        }
    }
}
