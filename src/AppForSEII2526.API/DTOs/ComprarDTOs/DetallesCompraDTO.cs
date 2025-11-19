

namespace AppForSEII2526.API.DTOs.ComprarDTOs
{
    public class DetallesCompraDTO 
    {
        public DetallesCompraDTO(string nombre, string apellido, DateTime fechaCompra, string concesionarioEntrega, double precioCompra, IList<ComprarItemDTO> compraItems)
        {
            FechaCompra = fechaCompra;
            Nombre = nombre;
            Apellido = apellido;
            ConcesionarioEntrga = concesionarioEntrega;
            PrecioCompra = precioCompra;
            ComprarItemDTOs = compraItems;

        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }

        [StringLength(20, ErrorMessage = "Nombre no puede ser mayor a 20 caracteres.", MinimumLength = 2)]
        public string Nombre { get; set; }

        [StringLength(40, ErrorMessage = "Apellidos no puede ser mayor a 40 caracteres.", MinimumLength = 2)]
        public string Apellido { get; set; }

        [StringLength(60, ErrorMessage = "El concesionario de entrega no puede tener más de 60 caracteres .", MinimumLength = 1)]
        public string ConcesionarioEntrga {  get; set; }

        public double PrecioCompra { get; set; }
        public IList<ComprarItemDTO> ComprarItemDTOs { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetallesCompraDTO dTO &&
                   FechaCompra == dTO.FechaCompra &&
                   Nombre == dTO.Nombre &&
                   Apellido == dTO.Apellido &&
                   ConcesionarioEntrga == dTO.ConcesionarioEntrga &&
                   PrecioCompra == dTO.PrecioCompra &&
                   ComprarItemDTOs.SequenceEqual(dTO.ComprarItemDTOs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(),FechaCompra,Apellido,Nombre,ConcesionarioEntrga,PrecioCompra,ComprarItemDTOs);
        }
    }
}
