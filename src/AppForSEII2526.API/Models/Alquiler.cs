namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        public Alquiler()
        {
        }
        public Alquiler( string concesionarioEntrega, DateTime fechaAlquiler, MetodoPagoTipos metodoPago, DateTime inicioAlquiler, DateTime finAlquiler, IList<AlquilerItem> alquilerItems, ApplicationUser applicationUser)
        {

           
            InicioAlquiler = inicioAlquiler;
            FinAlquiler = finAlquiler;
            FechaAlquiler = fechaAlquiler;
            ConcesionarioEntrega = concesionarioEntrega;
           
            AlquilerItems = alquilerItems;
            MetodoPago = metodoPago;
            ApplicationUser = applicationUser;

            Nombre = applicationUser?.Nombre;      
            Apellido = applicationUser?.Apellido;
        }

        [Key]
        public int Id { get; set; }

        public double Total { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime FechaAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinAlquiler { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Concesionario de entrega ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección de entrega")]
        [StringLength(20, ErrorMessage = "Maximo 20 caracteres,minimo 1", MinimumLength = 1)]
        
        public string ConcesionarioEntrega { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }
        [Display(Name = "Metodos de pago")]
        public MetodoPagoTipos MetodoPago { get; set; }

       
        public ApplicationUser ApplicationUser { get; set; }
        public IList<AlquilerItem> AlquilerItems { get; set; }

    }

    public enum MetodoPagoTipos
    {
       Visa,
        GooglePay,
        PayPal
    }
}