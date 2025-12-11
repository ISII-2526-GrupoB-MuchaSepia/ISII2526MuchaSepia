using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {

        public AlquilerParaCrearDTO Alquiler { get; private set; } = new AlquilerParaCrearDTO()
        {
            AlquilerItems = new List<AlquilerItemDTO>()
            //get: publico, cualquiera puede leer Alquiler
            //set: privado, solo esta clase puede modificar Alquiler
            //al crear un nuevo AlquilerStateContainer, ya tienes un carrito de alquiler vacio listo para usar
        };


        public decimal Total
            //no guarda el valor, lo calcula cada vez que lo lees
        {
            get
            {
                int numeroDias=(Alquiler.FinAlquiler - Alquiler.InicioAlquiler).Days;
                return Convert.ToDecimal(Alquiler.AlquilerItems.Sum(item => item.PrecioAlquiler * numeroDias));
            }
        }

        public event Action? OnChange; //Declara un evento al que otros componentes se pueden suscribir

        private void NotifyStateChanged() => OnChange?.Invoke(); //Método para notificar a la interfaz que el estado ha cambiado


        public void AñadirCocheParaAlquiler(CocheParaAlquilerDTO coche)
        {
            //añade un coche al carrito de alquiler si no está ya añadido
            if (!Alquiler.AlquilerItems.Any(item => item.Modelo == coche.Modelo))
                Alquiler.AlquilerItems.Add(new AlquilerItemDTO()
                {
                    Modelo = coche.Modelo,
                    Fabricante = coche.Fabricante,
                    PrecioAlquiler = coche.PrecioAlquiler,

                }
                );
            
                
            }

        public void BorrarCoche(AlquilerItemDTO item) //elimina un coche del carrito de alquiler
        {
            Alquiler.AlquilerItems.Remove(item);
        }

        public void VaciarCarro() //elimina todos los coches del carrito de alquiler
        {
            Alquiler.AlquilerItems.Clear();
        }

        public void AlquilerProcesado() //resetea el carrito de alquiler después de procesar el alquiler
        {
            Alquiler = new AlquilerParaCrearDTO()
            {
                AlquilerItems = new List<AlquilerItemDTO>()

            };
        

        
        }
    }
}
