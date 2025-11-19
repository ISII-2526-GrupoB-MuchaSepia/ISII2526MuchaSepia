using Humanizer.Localisation;

namespace AppForSEII2526.API.DTOs.CocheDTOs
{
    public class CocheParaCompraDTO
    {
        public CocheParaCompraDTO()
        {
        }

        public CocheParaCompraDTO(int id, string modelo, double precioCompra,string color, string tCompbustible,string fabricante)
        {
            Id = id;
            Modelo = modelo;
            PrecioCompra = precioCompra;
            Color = color;
            Tcombustible = tCompbustible;
            Fabricante = fabricante;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Modelo tiene un máximo de 50 caracteres", MinimumLength = 1)]
        public string Modelo { get; set; }

        [StringLength(50, ErrorMessage = "Color tiene un máximo de 50 caracteres", MinimumLength = 1)]
        public string Color { get; set; }

        [StringLength(20, ErrorMessage = "El tipo de combustible no puede ser mayor de 20 caracteres y menro que 1", MinimumLength = 1)]
        public string Tcombustible { get; set; }

        [StringLength(20, ErrorMessage = "Fabricantee no puede ser mayor de 20 caracteres y menro que 1", MinimumLength = 1)]
        public string Fabricante { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Precio minimo 1")]
        [Display(Name = "Precio compra")]
        public double PrecioCompra{get; set;}


        public override bool Equals(object? obj)
        {
            return obj is CocheParaCompraDTO dTO &&
                   Id == dTO.Id &&
                   Color == dTO.Color &&
                   Modelo == dTO.Modelo &&
                   Fabricante == dTO.Fabricante &&
                   PrecioCompra == dTO.PrecioCompra &&
                   Tcombustible == dTO.Tcombustible;
        }
        



        public override int GetHashCode()
        {
            return HashCode.Combine(Id,Color, Modelo, Fabricante, PrecioCompra, Tcombustible);
        }
    }
}
