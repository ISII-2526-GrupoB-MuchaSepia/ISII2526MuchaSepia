namespace AppForSEII2526.API.Models
{
    public class AlquilerItem
    {
        public AlquilerItem()
        {
        }

        //constructor que reciba coche y alquiler y los inicialice, ademas de inicializar las propiedades CocheId y AlquilerId con los ids de coche y alquiler respectivamente

        public AlquilerItem(Coche coche, Alquiler alquiler)
        {
            Coche = coche;
            CocheId = coche.Id;
            Alquiler = alquiler;
            AlquilerId = alquiler.Id;
        }

        //crear un constructor que reciba coche, alquiler y cantidad ya que llama al de arriba y ademas inicializa la cantidad

        public AlquilerItem(Coche coche, Alquiler alquiler, int cantidad) : this(coche, alquiler)
        {
            Cantidad = cantidad;
            
        }

        //constructor solo con los ids de coche y alquiler y la cantidad
        public AlquilerItem(int cocheId, int alquilerId, int cantidad)
        {
            CocheId = cocheId;
            AlquilerId = alquilerId;
            Cantidad = cantidad;
        }





        public Coche Coche { get; set; }

        public int CocheId { get; set; }


        public Alquiler Alquiler { get; set; }

        public int AlquilerId { get; set; }

       
        public int Cantidad { get; set; }
    }
}
