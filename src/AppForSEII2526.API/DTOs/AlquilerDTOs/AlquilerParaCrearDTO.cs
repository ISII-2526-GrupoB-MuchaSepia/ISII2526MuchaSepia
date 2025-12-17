using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerParaCrearDTO // se usa cuando un cliente crea un nuevo alquiler
    {
        // recibir los datos del alquiler que el cliente envía al confirmar.

        //5. solicita al usuario su nombre, apellidos, dirección y método de pago
        //(Visa, Google Pay o Paypal) y la cantidad de cada coche seleccionado, siendo todos datos
        //obligatorios.


        public AlquilerParaCrearDTO(string? nombreUsuario, string nombre, string apellido, string concesionarioEntrega, MetodoPagoTipos metodoPago, DateTime inicioAlquiler, DateTime finAlquiler, IList<AlquilerItemDTO> alquilerItems, DateTime fechaAlquiler, double total)
        {
            NombreUsuario = nombreUsuario;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre)); // validar que no sea nulo, asigna a Nombre un valor de nombre, a no ser que nombre sea null. En ese caso se lanza una excepcion
            Apellido = apellido ?? throw new ArgumentNullException(nameof(apellido)); // validar que no sea nulo
            ConcesionarioEntrega = concesionarioEntrega ?? throw new ArgumentNullException(nameof(concesionarioEntrega)); // validar que no sea nulo garantiza que el DTO siempre tenga datos validos
            MetodoPago = metodoPago;
            InicioAlquiler = inicioAlquiler;
            FinAlquiler = finAlquiler;
            AlquilerItems = alquilerItems ?? throw new ArgumentNullException(nameof(alquilerItems)); // validar que no sea nulo
            FechaAlquiler = fechaAlquiler;
            Total = total;
        }

        public AlquilerParaCrearDTO()
        {
            AlquilerItems = new List<AlquilerItemDTO>(); //crear objeto sin parametros, inicializa como lista vacia para que no sea nulo
        }

        public double Total { get; set; }
        
        public string? NombreUsuario { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]//multiLineText sugiere que en un formulario web podria ser un campo de texto con varias lineas
        [Required(ErrorMessage = "Debe indicar nombre y apellidos.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe indicar nombre y apellidos.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe indicar un concesionario de entrega.")]
        public string ConcesionarioEntrega { get; set; }

        public IList<AlquilerItemDTO> AlquilerItems { get; set; } //lista de coches alquilados
        [Required]
        public MetodoPagoTipos MetodoPago { get; set; }








        public override bool Equals(object? obj)
        {
            return obj is AlquilerParaCrearDTO dTO &&
                   Total == dTO.Total &&
                   NombreUsuario == dTO.NombreUsuario &&
                   InicioAlquiler == dTO.InicioAlquiler &&
                   FinAlquiler == dTO.FinAlquiler &&
                   FechaAlquiler == dTO.FechaAlquiler &&
                   ConcesionarioEntrega == dTO.ConcesionarioEntrega &&
                   Nombre == dTO.Nombre &&
                   Apellido == dTO.Apellido &&
                   AlquilerItems.SequenceEqual(dTO.AlquilerItems) &&
                   MetodoPago == dTO.MetodoPago;
        }
    }
}