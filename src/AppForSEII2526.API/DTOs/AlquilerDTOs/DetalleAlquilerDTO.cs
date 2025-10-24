using AppForSEII2526.API.Models;
using System.Numerics;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class DetalleAlquilerDTO : AlquilerParaCrearDTO // hereda de AlquilerParaCrearDTO

        //devolver al usuario el resumen del alquiler realizado.


         //7. El sistema muestra un recibo con la información del alquiler indicando el nombre,
        //apellidos y dirección del usuario, coches alquilados (indicando su modelo, fabricante,
        //precio y cantidad), método de pago utilizado, fecha de inicio y fecha de finalización del
        //alquiler, fecha en la que se realizó el alquiler y precio total del alquiler.
    {

        public DetalleAlquilerDTO(int id, DateTime fechaAlquiler,string nombre, string apellido, string concesionarioEntrega, DateTime inicioAlquiler, DateTime finAlquiler, MetodoPagoTipos metodoPago,IList <AlquilerItemDTO> alquilerItems)
           :  base(nombre, apellido, concesionarioEntrega, metodoPago, inicioAlquiler, finAlquiler, alquilerItems)

        {
            Id = id;
            FechaAlquiler = fechaAlquiler;
        }
        public int Id { get; set; }
        public DateTime FechaAlquiler { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetalleAlquilerDTO dTO &&
                base.Equals(obj) &&
                PrecioTotal == dTO.PrecioTotal &&
                   Id == dTO.Id &&
                  CompareDate(FechaAlquiler, dTO.FechaAlquiler);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, FechaAlquiler);
        }
    }
}
