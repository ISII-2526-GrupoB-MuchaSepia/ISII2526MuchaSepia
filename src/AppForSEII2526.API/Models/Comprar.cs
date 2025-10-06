namespace AppForSEII2526.API.Models
{
    public class Comprar
    {
        public Comprar()
        {
        }

        public Comprar(int id, string nombre, string apellido, string concesionarioEntrega,
                      DateTime fechaCompra, decimal precioCompra, MetodoPagoTipos metodoPago, ApplicationUser applicationUser)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            ConcesionarioEntrega = concesionarioEntrega;
            FechaCompra = fechaCompra;
            PrecioCompra = precioCompra;
            MetodoPago = metodoPago;
            ApplicationUser = applicationUser;
        }

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [Display(Name = "Concesionario de entrega")]
        public string ConcesionarioEntrega { get; set; }

        [Precision(10, 2)]
        [Display(Name = "Precio de compra")]
        public decimal PrecioCompra { get; set; }

        [Display(Name = "Fecha de compra")]
        public DateTime FechaCompra { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Método de pago")]
        public MetodoPagoTipos MetodoPago { get; set; }

        public enum MetodoPagoTipos
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }
    }
}
