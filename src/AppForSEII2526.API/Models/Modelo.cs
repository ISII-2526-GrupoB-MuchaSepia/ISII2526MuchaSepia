namespace AppForSEII2526.API.Models
{
    
  
    public class Modelo
    {
        public Modelo()
        {
        }

        public Modelo(string name)
        {
            Name = name;
        }

        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.", MinimumLength = 4)]
        public string Name { get; set; }

       
        public IList<Coche> Coches { get; set; } = new List<Coche>();

    }
}