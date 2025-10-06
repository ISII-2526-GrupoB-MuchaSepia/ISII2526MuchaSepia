

namespace AppForSEII2526.API.Models
{
    [Index(nameof(ClaseCoche), IsUnique = true)] // Índice único por la clase de coche
    public class Coche
    {
        public Coche()
        {
        }

        public Coche(
            string claseCoche, string color, string descripcion, string desplazamientoMotor,
            string tipoCombustible, string fabricante, decimal precioCompra, int cantidadCompra,
            int cantidadAlquiler, double precioAlquiler, string tamanoLlanta,
            TipoMantenimiento tiposdeMantenimiento)
        {
            ClaseCoche = claseCoche;
            Color = color;
            Descripcion = descripcion;
            DesplazamientoMotor = desplazamientoMotor;
            TipoCombustible = tipoCombustible;
            TipoDeMantenimiento = tiposdeMantenimiento;
            Fabricante = fabricante;
            PrecioCompra = precioCompra;
            CantidadCompra = cantidadCompra;
            CantidadAlquiler = cantidadAlquiler;
            PrecioAlquiler = precioAlquiler;
            TamanoLlanta = tamanoLlanta;
        }

        // Propiedades
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la clase de coche no puede tener más de 50 caracteres.")]
        [Display(Name = "Clase de coche")]
        public string ClaseCoche { get; set; }

        [StringLength(30, ErrorMessage = "El color no puede superar los 30 caracteres.")]
        public string Color { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Desplazamiento del motor")]
        public string DesplazamientoMotor { get; set; }

        [Display(Name = "Tipo de combustible")]
        public string TipoCombustible { get; set; }

        [Display(Name = "Fabricante")]
        public string Fabricante { get; set; }

        [Display(Name = "Tipo de mantenimiento")]
        public TipoMantenimiento TipoDeMantenimiento { get; set; }


        [Range(0.5, double.MaxValue, ErrorMessage = "El precio mínimo es 0.5")]
        [Display(Name = "Precio de compra")]
        [Precision(10, 2)]
        public decimal PrecioCompra { get; set; }

      
        [Range(0.5, 100, ErrorMessage = "El precio de alquiler debe estar entre 0.5 y 100")]
        [Display(Name = "Precio de alquiler")]
        public double PrecioAlquiler { get; set; }

        [Display(Name = "Cantidad para compra")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad mínima para compra es 0")]
        public int CantidadCompra { get; set; }

        [Display(Name = "Cantidad para alquiler")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima para alquiler es 1")]
        public int CantidadAlquiler { get; set; }

        [Display(Name = "Tamaño de llanta")]
        public string TamanoLlanta { get; set; }


        // Enumeración de tipos de mantenimiento
        public enum TipoMantenimiento
        {
            Aceite,
            Frenos,
            Neumaticos,
            Transmision,
            Refrigeracion,
            Suspension
        }

       
    }
}























