using System;
using AppForMovies.UIT.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;
using Xunit;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;


namespace AppForSEII2526.UIT.AlquilerCoches
{
    public class CUAlquilerCoches_UIT : UC_UIT
    {
        private SelectCochesParaAlquilar_PO selectCochesParaAlquilar_PO;

        private const int cocheId = 1;
        private const string cocheModelo1 = "Mercedes Clase C";
        private const string cocheColor1 = "Negro";
        private const string cocheFabricante1 = "Mercedes";
        private const string cocheTipoCombustible1 = "Diesel";
        private const string cochePrecio1 = "120";



        private const string cocheModelo2 = "Volkswagen Golf";
        private const string cocheColor2 = "Azul";
        private const string cocheFabricante2 = "Volkswagen";
        private const string cocheTipoCombustible2 = "Gasolina";
        private const string cochePrecio2 = "80";


        public CUAlquilerCoches_UIT(ITestOutputHelper output) : base(output)
        {
            selectCochesParaAlquilar_PO = new SelectCochesParaAlquilar_PO(_driver, output);

        }

        private void Precondicion()
        {
            Perform_login("gadea@uclm.es", "gadea1234");
        }

        private void PasosInicialesParaAlquilarCoche()
        {
            Precondicion(); //cumple la precondición: El usuario debe estar conectado como Cliente
            selectCochesParaAlquilar_PO.WaitForBeingVisible(By.Id("CreateAlquiler")); //espera que el menú esté visible
            _driver.FindElement(By.Id("CreateAlquiler")).Click(); //hace click en el menú para entrar al caso de uso: abre la pantalla donde comienza tu flujo de alquiler
        }

        [Theory]
        [InlineData(cocheModelo1, cocheColor1, cocheFabricante1, cocheTipoCombustible1, cochePrecio1,"Mercedes","")]      //filtro solo por modelo
        [InlineData(cocheModelo2, cocheColor2, cocheFabricante2, cocheTipoCombustible2, cochePrecio2,"", "100" )]      //filtro solo por precio

        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_filtrar_coches(string cocheModelo,string cocheColor, string cocheFabricante, string cocheTipoCombustible, string cochePrecio, string filtroModelo,string filtroPrecio)
        {

            //Arrange
            PasosInicialesParaAlquilarCoche();
            var expectedCoches = new List<string[]>
            {
                new string[] { cocheModelo, cocheColor, cocheFabricante, cocheTipoCombustible, cochePrecio },
            };
            //ACT

            selectCochesParaAlquilar_PO.BuscarCoches(filtroModelo, filtroPrecio); //filtra por modelo`parcial y precio

            //ASSERT

            Assert.True(selectCochesParaAlquilar_PO.ComprobarCochesMostrados(expectedCoches)); //comprueba que los coches mostrados son los esperados



        }


    }
}
