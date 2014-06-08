namespace CV.POS.Wpf.Common
{
    public static class Constants
    {
        public const int AdminRoleId = 1;
        public const int SalesPersonRoleId = 2;
        public const int FullRoleId = 3;

        public const string ShouldBeClosed = "close";
        public const string IsFirstInstance = "firstInstance";


        public const string Error1 = "Ingrese Usuario y Password.";
        public const string Error2 = "Su Usuario o Password son incorrectos. Ingrese sus datos nuevamente.";
        public const string Error3 = "Error: No pudo crearse la sesión. Comuniquese con el administrador del sistema";
        public const string Error4 = "Error: no se tiene un Rol especificado para este usuario. Comuniquese con el administrador del sistema";
        public const string Error5 = "No se puede iniciar sesión debido a que no se ha hecho el Cierre de Caja.";
        public const string Error6 = "Este dato es requerido.";
        public const string Error7 = "Este dato debe ser de tipo monetario (numeros 2 decimales como máximo).";
        public const string Error8 = "Para poder realizar la apertura de caja, esta tiene que estar cerrada. El usuario que aperturo la caja debe entrar a la aplicación y cerrar la caja para poder continuar.";
        public const string Error9 = "Ocurrio un error inesperado. Comuniquese con el adminsitrador del sistema.";
        public const string Error10 = "Este dato no debe contener números o símbolos.";
        public const string Error11 = "La unidad ingresada no existe.";
        public const string Error12 = "El producto seleccionado ya se encuentra en la lista de ventas, seleccione otro producto.";
        public const string Error13 = "Este dato debe ser de tipo numérico.";
        public const string Error14 = "Ingrese un RUC y Razón Social válidos.";

        public const string Msg1 = "Caja abierta :)";

        public const string Token1 = "t1";
        public const string Token2 = "t2";
        public const string Token3 = "t3";
        public const string Token4 = "t4";
        public const string Token5 = "t5";
        public const string Token6 = "t6";
        public const string Token7 = "t7";
        public const string Token8 = "t8";
        public const string Token9 = "t9";
        public const string Token10 = "t10";
        public const string Token11 = "t11";

        

        //numeric values with 2 decimal digits
        public const string MoneyRegEx = @"(^\d+(\.\d{1,2})?$)|(^(\.\d{1,2})?$)";
        public const string NumericRegEx = @"^\d+?$";
        public const string AlphabeticRegEx = @"^[a-zA-Z]*$";
        public const string AlphabeticUppercaseRegEx = @"^\u+?$";

        public static class TicketType
        {
            public const string NoDocument = "NoDocument";
            public const string Boleta = "Boleta";
            public const string Factura = "Factura";
        }
    }
}
