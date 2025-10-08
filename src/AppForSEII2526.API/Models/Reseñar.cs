namespace AppForSEII2526.API.Models
{
    
    public class Reseñar
    {
        public Reseñar()
        {
            ReseñarItems = new List<ReseñarItem>();
        }

        public Reseñar(int id, string usuario, string pais, string tipoConductor, DateTime creado, ApplicationUser applicationUser)
        {
            Id = id;
            Usuario = usuario;
            Pais = pais;
            TipoConductor = tipoConductor;
            Creado = creado;
            ApplicationUser = applicationUser;
            ReseñarItems = new List<ReseñarItem>();
 
        }

        public int Id { get; set; }
        public string Usuario { get; set; }
        [Display(Name = "País")]
        public string Pais { get; set; }
        [Display(Name = "Tipo de conductor")]
        public string TipoConductor { get; set; }
        [Display(Name = "Fecha de creación")]
        public DateTime Creado { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<ReseñarItem> ReseñarItems { get; set; }
       
    }
}
