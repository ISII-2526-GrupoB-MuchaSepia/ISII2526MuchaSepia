namespace AppForSEII2526.API.Models
{
    public class ApplicationUser : IdentityUser
    {


        public ApplicationUser()
        {
        }

        public ApplicationUser(string id, string nombre, string apellido, string nombreUsuario)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            UserName = nombreUsuario;
            Email = nombreUsuario;
        }

        [Display(Name = "Nombre")]
        public string? Nombre
        {
            get;
            set;
        }

        [Display(Name = "Apellido")]
        public string? Apellido
        {
            get;
            set;
        }



    }
}
