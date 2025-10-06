namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        public Alquiler()
        {
        }
        public Alquiler(string nombre, string apellido, string concesionarioEntrega, DateTime fechaAlquiler, PaymentMethodTypes metodoPago, DateTime inicioAlquiler, DateTime finAlquiler, IList<AlquilerItem> alquilerItems, ApplicationUser applicationUser)
        {

            Total = alquilerItems.Sum(ri => ri.Cantidad * (finAlquiler - inicioAlquiler).Days);

            InicioAlquiler = inicioAlquiler;
            FinAlquiler = finAlquiler;
            FechaAlquiler = fechaAlquiler;
            ConcesionarioEntrega = concesionarioEntrega;
            Nombre = nombre;
            Apellido = apellido;
            AlquilerItems = alquilerItems;
            MetodoPago = metodoPago;
            ApplicationUser = applicationUser;
        }

        public int Id { get; set; }

        public double Total { get; set; }

        public DateTime FechaAlquiler { get; set; }

        public DateTime InicioAlquiler { get; set; }
        public DateTime FinAlquiler { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Concesionario de entrega ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección de entrega")]
        public string ConcesionarioEntrega { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }
        [Display(Name = "Metodos de pago")]
        public PaymentMethodTypes MetodoPago { get; set; }

       
        public ApplicationUser ApplicationUser { get; set; }
        public IList<AlquilerItem> AlquilerItems { get; set; }

    }

    public enum PaymentMethodTypes
    {
        CreditCard,
        PayPal,
        Cash
    }
}