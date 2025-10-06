using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


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