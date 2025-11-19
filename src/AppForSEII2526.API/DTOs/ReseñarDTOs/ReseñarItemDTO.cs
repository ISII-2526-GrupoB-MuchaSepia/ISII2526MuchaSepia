namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    // DTO PARA LOS ITEMS DE UNA RESEÑA, REPRESENTA CADA VALORACION DE UN COCHE
    public class ReseñarItemDTO
    {
        // CONSTRUCTOR PARA INICIALIZAR TODOS LOS CAMPOS DEL ITEM DE RESEÑA
        public ReseñarItemDTO(int reseñarId, int cocheId, string cocheNombre, int calificacion, string descripcion = "")
        {
            ReseñarId = reseñarId; // ID DE LA RESEÑA A LA QUE PERTENECE EL ITEM
            CocheId = cocheId; // ID DEL COCHE VALORADO
            CocheNombre = cocheNombre; // NOMBRE O CLASE DEL COCHE (POR EJEMPLO, MODELO)
            Calificacion = calificacion; // CALIFICACION PONDERADA O PUNTUACION DEL COCHE
            Descripcion = descripcion; // DESCRIPCION OPCIONAL CON COMENTARIOS ADICIONALES
        }

        public int ReseñarId { get; set; } // PROPIEDAD ID RESEÑA

        public int CocheId { get; set; } // PROPIEDAD ID COCHE

        public string CocheNombre { get; set; } // NOMBRE DEL COCHE PARA MOSTRAR

        public int Calificacion { get; set; } // PUNTUACION NUMERICA DE LA RESEÑA

        public string? Descripcion { get; set; } // TEXTO EXPLICATIVO ADICIONAL (PUEDE SER NULL)

        // SOBREESCRIBE EQUALS PARA COMPARAR POR VALORES DE PROPIEDADES
        public override bool Equals(object? obj)
        {
            return obj is ReseñarItemDTO dto &&
                   ReseñarId == dto.ReseñarId &&
                   CocheId == dto.CocheId &&
                   CocheNombre == dto.CocheNombre &&
                   Calificacion == dto.Calificacion &&
                   Descripcion == dto.Descripcion;
        }

        // SOBREESCRIBE GETHASHCODE PARA QUE COINCIDA CON LA IGUALDAD DEFINIDA
        public override int GetHashCode()
        {
            return HashCode.Combine(ReseñarId, CocheId, CocheNombre, Calificacion, Descripcion);
        }
    }
}
