using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.Shared.CocheDTOs
{
    // DTO UTILIZADO PARA MOSTRAR COCHES DISPONIBLES PARA RESEÑAR
    public class CocheParaReseñarDTO
    {
        // CONSTRUCTOR VACÍO PARA SERIALIZACIÓN O FRAMEWORKS DE BINDING
        public CocheParaReseñarDTO() { }

        // CONSTRUCTOR CON TODOS LOS CAMPOS PRINCIPALES DEL DTO
        public CocheParaReseñarDTO(
            int id,
            string claseCoche,
            string color,
            string descripcion,
            string modelo)
        {
            Id = id;                       // IDENTIFICADOR ÚNICO DEL COCHE
            ClaseCoche = claseCoche;       // CLASE DEL COCHE (ej. Berlina, SUV)
            Color = color;                 // COLOR DEL VEHÍCULO
            Descripcion = descripcion;     // DESCRIPCIÓN DEL COCHE, USADA EN LA INTERFAZ
            Modelo = modelo;               // MODELO ASOCIADO AL COCHE
        }

        public int Id { get; set; }          // PROPIEDAD ID PARA IDENTIFICAR EL COCHE

        [StringLength(50, ErrorMessage = "La clase del coche no puede tener más de 50 caracteres")]
        public string ClaseCoche { get; set; }  // CLASE DEL COCHE CON VALIDACIÓN DE LONGITUD

        [StringLength(30, ErrorMessage = "El color no puede tener más de 30 caracteres")]
        public string Color { get; set; }        // COLOR CON VALIDACIÓN DE LONGITUD

        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres", MinimumLength = 4)]
        public string Descripcion { get; set; }  // DESCRIPCIÓN CON VALIDACIÓN DE RANGO DE LONGITUD

        [Display(Name = "Modelo")]
        public string Modelo { get; set; }       // MODELO DEL COCHE (Ej. "Mercedes Clase C")

        // SE SOBRESCRIBE EQUALS PARA COMPARAR OBJETOS POR VALORES DE SUS CAMPOS
        public override bool Equals(object? obj)
        {
            return obj is CocheParaReseñarDTO dto &&
                   Id == dto.Id &&
                   ClaseCoche == dto.ClaseCoche &&
                   Color == dto.Color &&
                   Descripcion == dto.Descripcion &&
                   Modelo == dto.Modelo;
        }

        // SE SOBRESCRIBE GETHASHCODE PARA COINCIDIR CON EQUALS
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ClaseCoche, Color, Descripcion, Modelo);
        }
    }
}
