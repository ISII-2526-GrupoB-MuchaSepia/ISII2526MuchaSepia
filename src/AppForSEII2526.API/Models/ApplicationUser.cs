using Microsoft.AspNetCore.Identity;
using System.Net;
using static AppForSEII2526.API.Models.Comprar;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

   

    // Constructor que inicializa un usuario con los valores proporcionados
    public ApplicationUser( string id,string nombre, string apellido, string nombreUsuario, string direccion)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        NombreUsuario = nombreUsuario;
        Email = nombreUsuario;
        Direccion = direccion;
        Metodos_Pagos = new List<MetodoPagoTipos>();

        
        Compras = new List<Comprar>();
        Alquileres = new List<Alquiler>();
        Resenas = new List<Reseñar>();

       
        Pais = "";
        Conductor = ConductorTipos.Novato;
    }



    public ApplicationUser()
    {
        Compras = new List<Comprar>();
        Alquileres = new List<Alquiler>();
        Resenas = new List<Reseñar>();
       
        
    }

    public ApplicationUser( string id,string nombre, string apellido, string nombreUsuario, List<MetodoPagoTipos> metodos_Pagos, string direccion,
            string pais,
           ConductorTipos conductor)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        UserName = nombreUsuario;
        Email = nombreUsuario;
        Metodos_Pagos = new List<MetodoPagoTipos>();
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

    [Display(Name = "Nombre de Uusuario")]
    public string? NombreUsuario
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








