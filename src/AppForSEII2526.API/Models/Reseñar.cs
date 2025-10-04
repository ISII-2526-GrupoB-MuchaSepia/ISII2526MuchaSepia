namespace AppForSEII2526.API.Models
{
    [Index(nameof(Reseñar), IsUnique = true)]
    public class Reseñar
    {
        public Reseñar()
        {
        }

        public Reseñar(int id, string usuario, string pais, string tipoConductor, DateTime creado, ApplicationUser applicationUser)
        {
            Id = id;
            Usuario = usuario;
            Pais = pais;
            TipoConductor = tipoConductor;
            Creado = creado;
            ApplicationUser = applicationUser;
        }

        public int Id { get; set; }
        public string Usuario { get; set; }
        [Display(Name = "País")]
        public string Pais { get; set; }
        [Display(Name = "Tipo de conductor")]
        public string TipoConductor { get; set; }
        [Display(Name = "Fecha de creación")]
        [DataType(DataType.DateTime)]
        public DateTime Creado { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
