

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerParaCrearDTO // se usa cuando un cliente crea un nuevo alquiler
    {
        // recibir los datos del alquiler que el cliente envía al confirmar.

        //5. solicita al usuario su nombre, apellidos, dirección y método de pago
        //(Visa, Google Pay o Paypal) y la cantidad de cada coche seleccionado, siendo todos datos
        //obligatorios.


        public AlquilerParaCrearDTO(string nombre, string apellido,string concesionarioEntrega, MetodoPagoTipos metodoPago, DateTime inicioAlquiler, DateTime finAlquiler,IList <AlquilerItemDTO> alquilerItems) {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre)); // validar que no sea nulo, asigna a Nombre un valor de nombre, a no ser que nombre sea null. En ese caso se lanza una excepcion
            Apellido = apellido ?? throw new ArgumentNullException(nameof(apellido)); // validar que no sea nulo
            ConcesionarioEntrega = concesionarioEntrega ?? throw new ArgumentNullException(nameof(concesionarioEntrega)); // validar que no sea nulo garantiza que el DTO siempre tenga datos validos
            MetodoPago = metodoPago;
            InicioAlquiler = inicioAlquiler;
            FinAlquiler = finAlquiler;
            AlquilerItems = alquilerItems ?? throw new ArgumentNullException(nameof(alquilerItems)); // validar que no sea nulo
        }

        public AlquilerParaCrearDTO()
        {
            AlquilerItems= new List<AlquilerItemDTO>(); //crear objeto sin parametros, inicializa como lista vacia para que no sea nulo
        }

        public DateTime InicioAlquiler { get; set; }
        public DateTime FinAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]//multiLineText sugiere que en un formulario web podria ser un campo de texto con varias lineas
        [Display(Name = "Concesionario de entrega")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Debe tener minimo 10 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, configure su concesionario de entrega")]
        public string ConcesionarioEntrega { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, escriba su nombre y apellido")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Nombre y apellido con minimo 10 caracteres")]
        public string Apellido { get; set; }

        public IList<AlquilerItemDTO> AlquilerItems { get; set; } //lista de coches alquilados
        [Required]
        public MetodoPagoTipos MetodoPago { get; set; }


        private int NumeroDias
        {
            get
            {
                return (FinAlquiler - InicioAlquiler).Days;
            }
        }
        [Display(Name = "Precio Total")]
        [JsonPropertyName("PrecioTotal")]
        public double PrecioTotal
        {
            get
            {
                return AlquilerItems.Sum(ri => ri.PrecioAlquiler * NumeroDias);
                //calcula el precio total del alquiler, suma el precio de todos los coches multiplicando el precio por los dias
                // 2coches -> 50 cada uno -> 3 dias = (50*2)*3=300
            }
        }



        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));

            //considera dos fechas iguales si su diferencia es menor de un minuto
        }
        public override bool Equals(object? obj)
        {
            return obj is AlquilerParaCrearDTO dTO &&
                CompareDate(InicioAlquiler, dTO.InicioAlquiler) &&
                CompareDate(FinAlquiler, dTO.FinAlquiler) &&
                     ConcesionarioEntrega == dTO.ConcesionarioEntrega &&
                     Nombre == dTO.Nombre &&
                     Apellido == dTO.Apellido &&
                    AlquilerItems.SequenceEqual(dTO.AlquilerItems) &&
                    MetodoPago == dTO.MetodoPago &&
                    PrecioTotal == dTO.PrecioTotal;



        }

    }
}
