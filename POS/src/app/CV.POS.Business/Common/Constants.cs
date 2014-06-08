namespace CV.POS.Business.Common
{
    public static class Constants
    {
        public static class ProductMovementCategory
        {
            public const string Sale = "Venta";
            public const string AddProducts = "AgregarProductos";
            public const string RetireProducts = "RetirarProductos";
        }

        public static class ProductMovementType
        {
            public const string In = "Entrada";
            public const string Out = "Salida";
        }

        public static class CashMovementCategory
        {
            public const string Sale = "Venta";
        }

        public static class CashMovementType
        {
            public const string In = "Entrada";
            public const string Out = "Salida";
        }

        public static class CashMovementStatus
        {
            public const string Ok = "Ok";
        }

        public static class SaleStatus
        {
            public const string Created = "Creado";
        }

        public static class SaleDocumentType
        {
            public const string NoDocument = "NoDocument";
            public const string Boleta = "Boleta";
            public const string Factura = "Factura";
        }

        public static class SaleDocumentStatus
        {
            public const string Created = "Creado";
        }
    }
}



