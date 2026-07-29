namespace MedStoreAPI.Common
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Central list of all stored procedure names used across the app.
    /// Repositories should reference these constants instead of hardcoding string
    /// literals, so a rename in the DB only needs updating in one place.
    /// </summary>
    public static class StoredProcedureNames
    {
        public static class Category
        {
            public const string Insert = "SP_CategoryInsert";
            public const string GetAll = "SP_CategoryGetAll";
            public const string Update = "SP_CategoryUpdate";
            public const string Delete = "SP_CategoryDelete";
        }

        public static class Unit
        {
            public const string Insert = "SP_UnitInsert";
            public const string GetAll = "SP_UnitGetAll";
        }

        public static class GSTSlab
        {
            public const string Insert = "SP_GSTSlabInsert";
            public const string GetAll = "SP_GSTSlabGetAll";
        }

        public static class PaymentMode
        {
            public const string Insert = "SP_PaymentModeInsert";
            public const string GetAll = "SP_PaymentModeGetAll";
        }

        public static class Store
        {
            public const string Insert = "SP_StoreInsert";
            public const string GetByID = "SP_StoreGetByID";
            public const string GetAll = "SP_StoreGetAll";
            public const string Update = "SP_StoreUpdate";
            public const string UpdateLogo = "SP_StoreUpdateLogo";
        }

        public static class User
        {
            public const string Insert = "SP_UserInsert";
            public const string GetByUsername = "SP_UserGetByUsername";
            public const string GetByID = "SP_UserGetByID";
            public const string GetByStore = "SP_UserGetByStore";
            public const string UpdatePassword = "SP_UserUpdatePassword";
            public const string Deactivate = "SP_UserDeactivate";
        }

        public static class Supplier
        {
            public const string Insert = "SP_SupplierInsert";
            public const string GetAll = "SP_SupplierGetAll";
            public const string GetByID = "SP_SupplierGetByID";
            public const string Update = "SP_SupplierUpdate";
            public const string Delete = "SP_SupplierDelete";
        }

        public static class Medicine
        {
            public const string Insert = "SP_MedicineInsert";
            public const string Update = "SP_MedicineUpdate";
            public const string GetByID = "SP_MedicineGetByID";
            public const string GetAll = "SP_MedicineGetAll";
            public const string Search = "SP_MedicineSearch";
            public const string Delete = "SP_MedicineDelete";
        }

        public static class Batch
        {
            public const string Insert = "SP_BatchInsert";
            public const string GetByID = "SP_BatchGetByID";
            public const string GetByMedicine = "SP_BatchGetByMedicine";
            public const string GetExpiryStatus = "SP_BatchGetExpiryStatus";
            public const string GetExpiring = "SP_BatchGetExpiring";
            public const string GetLowStock = "SP_BatchGetLowStock";
            public const string ReduceStock = "SP_BatchReduceStock";
            public const string Delete = "SP_BatchDelete";
        }

        public static class Customer
        {
            public const string Insert = "SP_CustomerInsert";
            public const string GetByMobile = "SP_CustomerGetByMobile";
            public const string GetAll = "SP_CustomerGetAll";
        }

        public static class Invoice
        {
            public const string Create = "SP_InvoiceCreate";
            public const string ItemInsertAndReduceStock = "SP_InvoiceItemInsertAndReduceStock";
            public const string GetByID = "SP_InvoiceGetByID";
            public const string GetByDateRange = "SP_InvoiceGetByDateRange";
            public const string Cancel = "SP_InvoiceCancel";
            public const string GetDailySummary = "SP_InvoiceGetDailySummary";
        }

        public static class CustomerCredit
        {
            public const string Insert = "SP_CustomerCreditInsert";
            public const string GetByID = "SP_CustomerCreditGetByID";
            public const string GetPending = "SP_CustomerCreditGetPending";
            public const string GetByCustomer = "SP_CustomerCreditGetByCustomer";
            public const string AddPayment = "SP_CustomerCreditAddPayment";
        }

        public static class Dashboard
        {
            public const string GetSummary = "SP_DashboardGetSummary";
        }
    }
}
