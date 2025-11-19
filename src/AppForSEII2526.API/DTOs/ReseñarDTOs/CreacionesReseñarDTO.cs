using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    // DTO PARA CREAR UNA NUEVA RESEÑA, INCLUYE DATOS DEL USUARIO Y LISTA DE ITEMS
    public class CreacionesReseñarDTO
    {
        // CONSTRUCTOR PRINCIPAL CON VALIDACIÓN DE NULOS
        public CreacionesReseñarDTO(
            string usuario,
            string tipoConductor,
            string pais,
            DateTime creado)
        {
            Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            TipoConductor = tipoConductor ?? throw new ArgumentNullException(nameof(tipoConductor));
            Pais = pais ?? throw new ArgumentNullException(nameof(pais));
            Creado = creado;
        }

        // CONSTRUCTOR VACÍO PARA SERIALIZACIÓN/DTO MODEL BINDING
        public CreacionesReseñarDTO()
        {
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres")]
        public string Usuario { get; set; }  // NOMBRE USUARIO QUE HACE LA RESEÑA

        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de conductor es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de conductor debe tener entre 3 y 50 caracteres")]
        public string TipoConductor { get; set; }  // TIPO DE CONDUCTOR ("Titular" o "Adicional")

        [Required(AllowEmptyStrings = false, ErrorMessage = "El país es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El país debe tener entre 3 y 50 caracteres")]
        public string Pais { get; set; }  // PAÍS DE LA RESEÑA

        [Required]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Display(Name = "Fecha de creación")]
        public DateTime Creado { get; set; } // FECHA EN QUE SE CREA LA RESEÑA

        // LISTA DE ITEMS DE LA RESEÑA, DEBE HABER AL MENOS UNO (VALIDACIÓN REQUERIDA)
        [Required(ErrorMessage = "Debes añadir al menos una reseña de coche.")]
        public List<ReseñarItemDTO> ReseñarItems { get; set; }

        // MÉTODO PARA COMPARAR FECHAS CON MARGEN DE 1 MINUTO
        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        // SOBREESCRIBIMOS EQUALS PARA COMPARAR OBJETOS POR VALORES DE PROPIEDADES IMPORTANTES
        public override bool Equals(object? obj)
        {
            return obj is CreacionesReseñarDTO dto &&
                   Usuario == dto.Usuario &&
                   TipoConductor == dto.TipoConductor &&
                   Pais == dto.Pais &&
                   CompareDate(Creado, dto.Creado);
        }

        // SOBREESCRIBIMOS GETHASHCODE PARA SOPORTAR USO EN COLECCIONES HASH O DICCIONARIOS
        public override int GetHashCode()
        {
            return HashCode.Combine(Usuario, TipoConductor, Pais, Creado);
        }
    }
}
