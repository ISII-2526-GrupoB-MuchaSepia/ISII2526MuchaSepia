namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ReseñarId), nameof(CocheId))]
    public class ReseñarItem
    {
        public ReseñarItem()
        {
        }

        public ReseñarItem(Coche coche, int rating, Reseñar reseñar, string descripcion)
        {
            Coche = coche;
            CocheId = coche.Id;
            Reseñar = reseñar;
            ReseñarId = reseñar.Id;
            Calificacion = calificacion;
            Descripcion = descripcion;
        }

        public Coche Coche { get; set; }

        public int CocheId { get; set; }

        public Reseñar Reseñar { get; set; }

        public int ReseñarId { get; set; }

        [Range(1, 5, ErrorMessage = "La calificacion debe estar entre 1 y 5")]
        public int Calificacion { get; set; }
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
