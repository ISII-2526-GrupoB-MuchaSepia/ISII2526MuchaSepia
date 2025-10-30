namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    public class DetallesReseñarDTO : CreacionesReseñarDTO
    {
        public DetallesReseñarDTO(
            int id,
            DateTime creado,
            string usuario,
            string pais,
            string tipoConductor,
            ApplicationUser applicationUser,
            IList<ReseñarItemDTO> reseñarItems)
            : base(usuario, tipoConductor, pais, creado)
        {
            Id = id;
            Creado = creado;
            ReseñarItems = reseñarItems;
        }

        public int Id { get; set; }

        public DateTime Creado { get; set; }

        public IList<ReseñarItemDTO> ReseñarItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetallesReseñarDTO dto &&
                   base.Equals(obj) &&
                   Id == dto.Id &&
                   CompareDate(Creado, dto.Creado);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Creado);
        }
    }
}
