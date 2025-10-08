using Microsoft.AspNetCore.Identity;
using static AppForSEII2526.API.Models.Comprar;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


    public ApplicationUser()
    {
        Compras = new List<Comprar>();
        Alquileres = new List<Alquiler>();
        Resenas = new List<Reseñar>();
    }

    public ApplicationUser(string id, string nombre, string apellido, string nombreUsuario, List<MetodoPagoTipos> metodos_Pagos, string direccion,
            string pais,
           ConductorTipos conductor)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        UserName = nombreUsuario;
        Email = nombreUsuario;
        Metodos_Pagos = metodos_Pagos;
        Compras = new List<Comprar>();
        Alquileres= new List<Alquiler>();
        Resenas = new List<Reseñar>();
        Direccion = direccion;
        Pais = pais;
        Conductor = conductor;

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

    [Required]
    [Display(Name = "Métodos de Pago")]
    public List<MetodoPagoTipos> Metodos_Pagos { get; set; }

    [Display(Name = "Reseñas")]
    public IList<Reseñar> Resenas { get; set; }

    [Display(Name = "Compras")]
    public List<Comprar> Compras { get; set; }

    [Display(Name = "Alquileres")]
    public List<Alquiler> Alquileres { get; set; }


    [Display(Name = "Dirección")]
    [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
    public string Direccion { get; set; }


    [Display(Name = "País")]
    [StringLength(100, ErrorMessage = "El nombre del país no puede superar los 100 caracteres.")]
    public string Pais { get; set; }

    [Display(Name = "Tipo de Conductor")]
    public ConductorTipos Conductor { get; set; }


  

}

public enum ConductorTipos
{
    Novato,
    Experto
}








