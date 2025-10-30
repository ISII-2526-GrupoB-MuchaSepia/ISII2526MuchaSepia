namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    public class ReseñarItemDTO
    {
        public ReseñarItemDTO(int reseñarId, int cocheId, string cocheNombre, int rating, string descripcion = "")
        {
            ReseñarId = reseñarId;
            CocheId = cocheId;
            CocheNombre = cocheNombre;
            Calificacion  = calificacion;
            Descripcion = descripcion;
        }

        public int ReseñarId { get; set; }

        public int CocheId { get; set; }

        public string CocheNombre { get; set; }

        public int Calificacion  { get; set; }

        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReseñarItemDTO dto &&
                   ReseñarId == dto.ReseñarId &&
                   CocheId == dto.CocheId &&
                   CocheNombre == dto.CocheNombre &&
                   Calificacion  == dto.Calificacion  &&
                   Descripcion == dto.Descripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReseñarId, CocheId, CocheNombre, Rating, Descripcion);
        }
    }
}
