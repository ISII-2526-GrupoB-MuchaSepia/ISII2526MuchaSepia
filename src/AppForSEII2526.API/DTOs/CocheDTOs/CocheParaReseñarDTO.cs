using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.Shared.CocheDTOs
{
    public class CocheParaReseñarDTO
    {
        public CocheParaReseñarDTO() { }

        public CocheParaReseñarDTO(
            int id,
            string claseCoche,
            string color,
            string descripcion,
            string modelo)
        {
            Id = id;
            ClaseCoche = claseCoche;
            Color = color;
            Descripcion = descripcion;
            Modelo = modelo;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "La clase del coche no puede tener más de 50 caracteres")]
        public string ClaseCoche { get; set; }

        [StringLength(30, ErrorMessage = "El color no puede tener más de 30 caracteres")]
        public string Color { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres", MinimumLength = 4)]
        public string Descripcion { get; set; }

        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CocheParaReseñarDTO dto &&
                   Id == dto.Id &&
                   ClaseCoche == dto.ClaseCoche &&
                   Color == dto.Color &&
                   Descripcion == dto.Descripcion &&
                   Modelo == dto.Modelo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ClaseCoche, Color, Descripcion, Modelo);
        }
    }
}
