namespace AppForSEII2526.API.DTOs.ReseñarDTOs
{
    // DTO PARA DETALLES COMPLETOS DE UNA RESEÑA, HEREDA PROPIEDADES Y VALIDACIONES DE CreacionesReseñarDTO
    public class DetallesReseñarDTO : CreacionesReseñarDTO
    {
        // CONSTRUCTOR PARA INICIALIZAR EL DTO CON DATOS COMPLETOS, INCLUYE ID Y LISTA DE ITEMS
        public DetallesReseñarDTO(
            int id,
            DateTime creado,
            string usuario,
            string pais,
            string tipoConductor,
            ApplicationUser applicationUser,
            IList<ReseñarItemDTO> reseñarItems)
            : base(usuario, tipoConductor, pais, creado) // LLAMA AL CONSTRUCTOR BASE PARA ASIGNAR PROPIEDADES DE USUARIO ETC.
        {
            Id = id;
            Creado = creado; // SOBRESCRIBE SI ES NECESARIO
            ReseñarItems = reseñarItems; // ASIGNA LISTA DETALLADA DE ITEMS
        }

        public int Id { get; set; } // IDENTIFICADOR ÚNICO DE LA RESEÑA

        public DateTime Creado { get; set; } // FECHA DE CREACIÓN (POSIBLEMENTE DUPLICADA DEL BASE PARA SOBRESCRIBIR)

        public IList<ReseñarItemDTO> ReseñarItems { get; set; } // LISTA DE ITEMS DE LA RESEÑA (COCHES, CALIFICACIONES, ETC)

        // SOBRESCRIBE EQUALS PARA COMPARAR OBJETOS INCLUYENDO CAMPOS ADICIONALES
        public override bool Equals(object? obj)
        {
            return obj is DetallesReseñarDTO dto &&
                   base.Equals(obj) && // COMPARA CAMPOS DEL DTO BASE (usuario, tipoConductor, pais, fecha)
                   Id == dto.Id && // COMPARA ID
                   CompareDate(Creado, dto.Creado); // COMPARA FECHA CON MARGEN DE 1 MINUTO
        }

        // SOBRESCRIBE GETHASHCODE PARA COINCIDIR CON EQUALS
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Creado);
        }
    }
}
