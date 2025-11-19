using AppForSEII2526.API.Models;
using System.Numerics;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class DetalleAlquilerDTO  // hereda de AlquilerParaCrearDTO

    //devolver al usuario el resumen del alquiler realizado.


    //7. El sistema muestra un recibo con la información del alquiler indicando el nombre,
    //apellidos y dirección del usuario, coches alquilados (indicando su modelo, fabricante,
    //precio y cantidad), método de pago utilizado, fecha de inicio y fecha de finalización del
    //alquiler, fecha en la que se realizó el alquiler y precio total del alquiler.
    {

        public DetalleAlquilerDTO( DateTime fechaAlquiler, string nombre, string apellido, string concesionarioEntrega, DateTime inicioAlquiler, DateTime finAlquiler, MetodoPagoTipos metodoPago, double total, IList<AlquilerItemDTO> alquilerItems)
           

        {
            
            FechaAlquiler = fechaAlquiler;
            Nombre = nombre;
            Apellido = apellido;
            ConcesionarioEntrega = concesionarioEntrega;
            InicioAlquiler = inicioAlquiler;
            FinAlquiler = finAlquiler;
            MetodoPago = metodoPago;
            Total = total;
            AlquilerItems = alquilerItems;
            
        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime InicioAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinAlquiler { get; set; }
        
        
        [Required]
public MetodoPagoTipos MetodoPago { get; set; }

        public IList<AlquilerItemDTO> AlquilerItems { get; set; }

        [StringLength(20, ErrorMessage = "El nombre no puede tener más de 25 caracteres ni menos de 1.")]
        public string Nombre { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 40 caracteres ni menos de 1.")]
        public string Apellido { get; set; }

        [StringLength(50, ErrorMessage = "El concesionario de entrega no puede tener más de 35 caracteres ni menos de 1.")]
        public string ConcesionarioEntrega { get; set; }

        public double Total { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetalleAlquilerDTO dTO &&
                  
                   FechaAlquiler == dTO.FechaAlquiler &&
                   InicioAlquiler == dTO.InicioAlquiler &&
                   FinAlquiler == dTO.FinAlquiler &&
                   MetodoPago == dTO.MetodoPago &&
                   AlquilerItems.SequenceEqual(dTO.AlquilerItems) &&
                   Nombre == dTO.Nombre &&
                   Apellido == dTO.Apellido &&
                   ConcesionarioEntrega == dTO.ConcesionarioEntrega &&
                   Total == dTO.Total;
        }
    }
}