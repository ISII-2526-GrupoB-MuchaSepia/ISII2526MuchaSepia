using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    public class CreacionesReseñarDTO
    {
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

        public CreacionesReseñarDTO()
        {
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres")]
        public string Usuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de conductor es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de conductor debe tener entre 3 y 50 caracteres")]
        public string TipoConductor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El país es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El país debe tener entre 3 y 50 caracteres")]
        public string Pais { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Display(Name = "Fecha de creación")]
        public DateTime Creado { get; set; }

        // Cambio obligatorio para que se puedan enviar varios items en una reseña
        [Required(ErrorMessage = "Debes añadir al menos una reseña de coche.")]
        public List<ReseñarItemDTO> ReseñarItems { get; set; }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is CreacionesReseñarDTO dto &&
                   Usuario == dto.Usuario &&
                   TipoConductor == dto.TipoConductor &&
                   Pais == dto.Pais &&
                   CompareDate(Creado, dto.Creado);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Usuario, TipoConductor, Pais, Creado);
        }
    }
}
