-- =====================================================
-- Author: Mahesh Kumar
-- Date: 26/07/2026
-- Description: Seed data script - a few sample records per table, inserted
-- in FK-safe order, so you can start testing the API immediately without
-- manually creating master data first.
-- Run this AFTER all tables + stored procedures have been created.
-- =====================================================

-- 1. ROLES
INSERT INTO Roles (RoleName, Description, IsActive) VALUES
('Owner', 'Store owner with full access', 1),
('Pharmacist', 'Can manage inventory and billing', 1),
('Cashier', 'Can only do billing', 1);

-- 2. CATEGORIES
INSERT INTO Categories (CategoryName, ParentCategoryId, IsActive) VALUES
('Tablet', NULL, 1),
('Syrup', NULL, 1),
('Injection', NULL, 1),
('Surgical', NULL, 1);

-- 3. UNITS
INSERT INTO Units (UnitName, IsActive) VALUES
('Strip', 1),
('Bottle', 1),
('Box', 1),
('Piece', 1);

-- 4. GST SLABS
INSERT INTO GSTSlabs (Percentage, IsActive) VALUES
(0.00, 1),
(5.00, 1),
(12.00, 1),
(18.00, 1);

-- 5. PAYMENT MODES
INSERT INTO PaymentModes (ModeName, IsActive) VALUES
('Cash', 1),
('UPI', 1),
('Card', 1),
('Udhaar', 1);

-- 6. STORES
INSERT INTO Stores (StoreName, Address, City, State, Pincode, GSTIN, DrugLicenseNumber, ContactNumber, Email, IsActive) VALUES
('TechTalk Medical Store', '123 MG Road', 'Lucknow', 'Uttar Pradesh', '226001', '09ABCDE1234F1Z5', 'DL-UP-2024-00123', '9876543210', 'store@techtalkmedical.com', 1);

-- 7. USERS
-- =====================================================
-- IMPORTANT: Do NOT insert Users directly via SQL. Passwords must be hashed
-- using BCrypt (handled by UsersService), which a plain SQL INSERT cannot do.
-- Instead, create users through the API:
--   POST /api/Auth/register
--   {
--     "storeID": 1,
--     "roleID": 1,
--     "fullName": "Mahesh Kumar",
--     "username": "mahesh.owner",
--     "password": "YourStrongPassword123",
--     "email": "mahesh@techtalkmedical.com",
--     "mobile": "9876543210"
--   }
-- Then log in via POST /api/Auth/login to get a JWT token.
-- See the Postman collection's "Auth" folder for ready-made requests.
-- =====================================================

-- 8. SUPPLIERS
INSERT INTO Suppliers (StoreId, SupplierName, ContactPerson, ContactNumber, Email, Address, ReturnPolicyDays, GSTIN, IsActive) VALUES
(1, 'Sun Pharma Distributors', 'Amit Verma', '9812345678', 'amit@sunpharmadist.com', 'Sector 10, Lucknow', 90, '09XXXXX1111X1Z1', 1),
(1, 'Cipla Wholesale Agency', 'Neha Gupta', '9812345679', 'neha@ciplawholesale.com', 'Hazratganj, Lucknow', 60, '09XXXXX2222X1Z2', 1);

-- 9. MEDICINES
INSERT INTO Medicines (StoreId, Name, Composition, Manufacturer, CategoryId, UnitId, GSTSlabId, HSNCode, PrescriptionRequired, ReorderPoint, MaxStockLevel, IsActive) VALUES
(1, 'Paracetamol 500mg', 'Paracetamol 500mg', 'Sun Pharma', 1, 1, 2, '30049099', 0, 50, 300, 1),
(1, 'Amoxicillin 250mg', 'Amoxicillin 250mg', 'Cipla', 1, 1, 3, '30041020', 1, 30, 200, 1),
(1, 'Cough Syrup', 'Dextromethorphan', 'Cipla', 2, 2, 3, '30049011', 0, 20, 100, 1),
(1, 'ORS Sachet', 'Oral Rehydration Salts', 'Sun Pharma', 1, 4, 1, '30049023', 0, 40, 250, 1);

-- 10. BATCHES
-- Note: SP_BatchInsert auto-sets QuantityRemaining = QuantityReceived on insert.
-- Direct INSERT here (bypassing SP for seeding speed) sets it explicitly too.
INSERT INTO Batches (StoreId, MedicineId, SupplierId, BatchNumber, ExpiryDate, ManufactureDate, QuantityReceived, QuantityRemaining, PurchasePrice, MRP, DateReceived, IsActive) VALUES
(1, 1, 1, 'PCM-B001', DATEADD(MONTH, 18, GETDATE()), DATEADD(MONTH, -2, GETDATE()), 200, 200, 15.00, 22.00, GETDATE(), 1),
(1, 1, 1, 'PCM-B002', DATEADD(DAY, 25, GETDATE()), DATEADD(MONTH, -10, GETDATE()), 50, 50, 14.50, 22.00, DATEADD(DAY, -20, GETDATE()), 1),
(1, 2, 2, 'AMX-B001', DATEADD(MONTH, 12, GETDATE()), DATEADD(MONTH, -1, GETDATE()), 100, 100, 45.00, 65.00, GETDATE(), 1),
(1, 3, 2, 'CGH-B001', DATEADD(DAY, 15, GETDATE()), DATEADD(MONTH, -8, GETDATE()), 30, 30, 60.00, 85.00, DATEADD(DAY, -30, GETDATE()), 1),
(1, 4, 1, 'ORS-B001', DATEADD(MONTH, 20, GETDATE()), DATEADD(MONTH, -1, GETDATE()), 150, 150, 8.00, 12.00, GETDATE(), 1);

-- 11. CUSTOMERS
INSERT INTO Customers (StoreId, Name, Mobile, Address, IsActive) VALUES
(1, 'Ramesh Yadav', '9911223344', 'Aliganj, Lucknow', 1),
(1, 'Sunita Devi', '9911223355', 'Indira Nagar, Lucknow', 1),
(1, 'Walk-in Customer', '0000000000', NULL, 1);

-- =====================================================
-- Note on Invoices / InvoiceItems / CustomerCredits / CreditPayments:
-- These should be created THROUGH THE API (POST /api/Invoices), not by
-- direct INSERT, because:
--   - Invoice creation must go through the transaction in InvoicesRepository
--     (header + items + stock reduction happen together).
--   - GST amounts / line totals are calculated server-side by InvoicesService.
-- Use the Postman collection's "Invoices > Create Invoice" request with the
-- BatchIDs/MedicineIDs seeded above (BatchID 1-5, MedicineID 1-4) to generate
-- realistic Invoice + InvoiceItem + (optionally) CustomerCredit records.
-- =====================================================
