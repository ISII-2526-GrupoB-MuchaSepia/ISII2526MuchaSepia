using AppForSEII2526.API.Models;
using System.Drawing;

namespace AppForSEII2526.API.DTOs.CocheDTOs
{
    public class CocheParaAlquilerDTO
    //MOSTRAR COCHES DISPONIBLES PARA ALQUILER (PASO 2 CASO DE USO)
    //2. El sistema muestra el conjunto de coches disponibles para alquilar desde hoy hasta la
    //próxima semana, indicando el nombre del modelo, el tipo de gasoil, el fabricante, el
    //precio del alquiler y el color.

    {

        public CocheParaAlquilerDTO()
        {

        }

        public CocheParaAlquilerDTO(int id,string modelo,string color, double precioAlquiler, string tipoCombustible, string fabricante)
        {
            Id = id;
            Modelo = modelo;
            Color = color;
            PrecioAlquiler = precioAlquiler;
            TipoCombustible = tipoCombustible;
            Fabricante = fabricante;


        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El modelo del coche tiene un maximo de 50 caracteres")]
        public string Modelo { get; set; }


        [StringLength(50, ErrorMessage = "El color del coche debe tener entre 4 y 50 caracteres", MinimumLength = 4)]
        public string Color { get; set; }


        [StringLength(50, ErrorMessage = "El tipo de combustible del coche debe tener entre 4 y 50 caracteres", MinimumLength = 4)]
        public string TipoCombustible { get; set; }


        [StringLength(50, ErrorMessage = "El fabricante del coche debe tener entre 4 y 50 caracteres", MinimumLength = 4)]
        public string Fabricante { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Precio minimo es 1 ")]
        [Display(Name = "Precio de Alquiler")]
        public double PrecioAlquiler { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is CocheParaAlquilerDTO dTO &&
                   Id == dTO.Id &&
                   Modelo == dTO.Modelo &&
                   Color == dTO.Color &&
                   PrecioAlquiler == dTO.PrecioAlquiler &&
                   TipoCombustible == dTO.TipoCombustible &&
                   Fabricante == dTO.Fabricante;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Modelo, Color, PrecioAlquiler, TipoCombustible, Fabricante);
        }

    }
}
