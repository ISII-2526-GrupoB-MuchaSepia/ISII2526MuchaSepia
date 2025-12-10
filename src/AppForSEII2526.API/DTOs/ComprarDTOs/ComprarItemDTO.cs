using System.Drawing;

namespace AppForSEII2526.API.DTOs.ComprarDTOs
{
    public class ComprarItemDTO
    {
        public ComprarItemDTO(int cocheID, string modelo, double precioCompra,int cantidad,string color, string descripcion)
        {
            CocheID = cocheID;
            PrecioCompra = precioCompra;
            Modelo = modelo;
            Cantidad = cantidad;
            Color = color;
            Description = descripcion;
        }

        public int CocheID { get; set; }

        public double PrecioCompra { get; set; }

        public string? Description { get; set; }

        public int Cantidad { get; set; }

        public string Modelo { get; set; }

        public string Color { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is ComprarItemDTO dTO &&
                   CocheID == dTO.CocheID &&
                   PrecioCompra == dTO.PrecioCompra &&
                   Cantidad == dTO.Cantidad &&
                   Modelo == dTO.Modelo &&
                   Color == dTO.Color;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CocheID, PrecioCompra, Cantidad, Modelo, Color);
        }
    }
}
