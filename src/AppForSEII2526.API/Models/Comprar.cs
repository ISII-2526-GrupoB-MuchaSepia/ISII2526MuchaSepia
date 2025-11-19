namespace AppForSEII2526.API.Models
{
    public class Comprar
    {
        public Comprar()
        {
            ComprarItems = new List<ComprarItem>();
        }

        

        public Comprar(int comprarId, string nombre, string apellido, ApplicationUser applicationUser, string concesionarioEntrega, DateTime fechaCompra, IList<ComprarItem> comprarItems, MetodoPagoTipos metodoPago) :
            this(nombre, apellido, applicationUser, concesionarioEntrega, fechaCompra, comprarItems, metodoPago)
        {
            Id = comprarId;

        }
        public Comprar(string nombre, string apellido, ApplicationUser applicationUser, string concesionarioEntrega, DateTime fechaCompra, IList<ComprarItem> comprarItems, MetodoPagoTipos metodoPago)
        {

            

            Nombre = nombre;
            Apellido = apellido;
            ApplicationUser = applicationUser;
            ConcesionarioEntrega = concesionarioEntrega;
            FechaCompra = fechaCompra;
            ComprarItems = comprarItems;
            MetodoPago = metodoPago;
        }



        public int Id { get; set; }

        [Precision(10, 2)]
        public double PrecioTotal { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [Display(Name = "Concesionario de entrega")]
        public string ConcesionarioEntrega { get; set; }

        [Precision(10, 2)]
        [Display(Name = "Precio de compra")]
        public double PrecioCompra { get; set; }

        [Display(Name = "Fecha de compra")]
        public DateTime FechaCompra { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Método de pago")]
        public MetodoPagoTipos MetodoPago { get; set; }

        public IList<ComprarItem> ComprarItems { get; set; }

        public enum MetodoPagoTipos
        {
            GooglePlay,
            Visa,
            
        }
    }
}
