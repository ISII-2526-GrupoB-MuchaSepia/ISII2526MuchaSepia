using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Shared; // Asegúrate de tener este using o el namespace correcto

namespace AppForSEII2526.UIT.CU_Reseñar
{
    public class CreacionesReseñar_PO : PageObject
    {
        // Campos del cliente / reseña
        private By _usuarioBy = By.Id("Usuario");
        private By _paisBy = By.Id("Pais");
        private By _tipoConductorBy = By.Id("TipoConductor");

        // Botones
        private By _publicarReseñasBtnBy = By.Id("Submit");
        private By _modificarCochesBtnBy = By.Id("ModifyCar");

        // Tabla de items reseñados (resumen)
        private By _tablaItemsBy = By.Id("TableOfReviewItems");

        // Helpers
        private IWebElement _usuario() => _driver.FindElement(_usuarioBy);
        private IWebElement _pais() => _driver.FindElement(_paisBy);
        private IWebElement _tipoConductor() => _driver.FindElement(_tipoConductorBy);

        public CreacionesReseñar_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Rellenar datos generales de la reseña
        public void RellenarDatosReseña(string usuario, string pais, string tipoConductor)
        {
            WaitForBeingVisible(_usuarioBy);

            // Borramos y escribimos Usuario
            var inputUsuario = _usuario();
            inputUsuario.Clear();
            inputUsuario.SendKeys(usuario);

            // Borramos y escribimos País (Vital para borrar "España" por defecto)
            var inputPais = _pais();
            inputPais.Clear();
            inputPais.SendKeys(pais);

            // Seleccionamos tipo conductor
            var inputConductor = _tipoConductor();
            inputConductor.SendKeys(tipoConductor);
        }

        // Rellenar descripción y calificación para un coche concreto
        public void RellenarDatosCoche(string descripcion, int calificacion, string carId)
        {
            By descripcionBy = By.Id("description_" + carId);
            By calificacionBy = By.Id("rating_" + carId);

            WaitForBeingClickable(descripcionBy);
            _driver.FindElement(descripcionBy).SendKeys(descripcion);

            WaitForBeingClickable(calificacionBy);
            var inputRating = _driver.FindElement(calificacionBy);
            inputRating.Clear();
            inputRating.SendKeys(calificacion.ToString());
        }

        // Pulsar botón para publicar reseñas
        public void PulsarPublicarReseñas()
        {
            _driver.FindElement(_publicarReseñasBtnBy).Click();
        }

        // Pulsar botón para modificar coches
        public void PulsarModificarCoches()
        {
            _driver.FindElement(_modificarCochesBtnBy).Click();
        }

        // Comprobar tabla de items de reseña
        public bool ComprobarListaDeItems(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, _tablaItemsBy);
        }

        // Verificación de errores de validación
        public bool ComprobarErrorValidación(string mensajeEsperado)
        {
            Thread.Sleep(2000); // Pequeña espera para que aparezca el error
            return _driver.PageSource.Contains(mensajeEsperado);
        }
    }
}