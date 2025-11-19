using System.Drawing;

namespace AppForSEII2526.API.DTOs.ComprarDTOs
{
    public class CreacionComprasDTO
    {
        public CreacionComprasDTO(double precioCompra, string nombre, string apellido, string concesionarioEntrega, MetodoPagoTipos metodoPago, IList<ComprarItemDTO> comprarItems)
        {
            PrecioCompra = precioCompra;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(Nombre));
            Apellido = apellido ?? throw new ArgumentNullException(nameof(Apellido));
            ConcesionarioEntrega = concesionarioEntrega ?? throw new ArgumentNullException(nameof(ConcesionarioEntrega));
            MetodoPago = metodoPago;
            ComprarItems = comprarItems ?? throw new ArgumentNullException(nameof(ComprarItems));
        }
        public double PrecioCompra { get; set; }

        [StringLength(20, ErrorMessage = "Nombre no puede tener mas de 20 cararcteres y menos de 2.", MinimumLength = 2)]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "Apellido no pude tener mas de 50 caracteres y  menos de 2.", MinimumLength = 2)]
        public string Apellido { get; set; }

        public string UserName { get; set; }

        [StringLength(20, ErrorMessage = "ConcesionarioEntrega no pude tener mas de 20 caracteres .", MinimumLength = 1)]
        public string ConcesionarioEntrega { get; set; }

        public MetodoPagoTipos MetodoPago { get; set; }

        public IList<ComprarItemDTO> ComprarItems { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is CreacionComprasDTO dTO &&
                   ConcesionarioEntrega == dTO.ConcesionarioEntrega &&
                   Nombre == dTO.Nombre &&
                   Apellido == dTO.Apellido &&
                   ComprarItems.SequenceEqual(dTO.ComprarItems) &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioCompra == dTO.PrecioCompra;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConcesionarioEntrega, Nombre, Apellido, ComprarItems, MetodoPago, PrecioCompra);
        }
    }
}
