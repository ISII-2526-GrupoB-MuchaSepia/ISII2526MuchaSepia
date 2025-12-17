using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reseñar
{
    public class DetailReseñar_PO : PageObject
    {
        public DetailReseñar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReviewDetail(string usuario, string pais, string tipoConductor, DateTime creado)
        {
            WaitForBeingVisible(By.Id("Creado"));

            bool resultado = true;

            // VERIFICACIÓN DE TEXTOS 
            string userReal = _driver.FindElement(By.Id("Usuario")).Text;
            if (!userReal.Contains(usuario))
            {
                _output.WriteLine($"FALLO USUARIO: Esperado '{usuario}', Encontrado '{userReal}'");
                resultado = false;
            }

            string paisReal = _driver.FindElement(By.Id("Pais")).Text;
            if (!paisReal.Contains(pais))
            {
                _output.WriteLine($"FALLO PAIS: Esperado '{pais}', Encontrado '{paisReal}'");
                resultado = false;
            }

            string conductorReal = _driver.FindElement(By.Id("TipoConductor")).Text;
            if (!conductorReal.Contains(tipoConductor))
            {
                _output.WriteLine($"FALLO CONDUCTOR: Esperado '{tipoConductor}', Encontrado '{conductorReal}'");
                resultado = false;
            }

            //VERIFICACIÓN DE FECHA 
            string fechaTexto = _driver.FindElement(By.Id("Creado")).Text;
            try
            {
                // Parseamos la fecha que muestra la web
                var actualReviewDate = DateTime.ParseExact(fechaTexto, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

               
                if (actualReviewDate.Date != creado.Date)
                {
                    _output.WriteLine($"FALLO FECHA: Las fechas no coinciden. Web: {actualReviewDate.ToShortDateString()}, Test: {creado.ToShortDateString()}");
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"ERROR PARSEANDO FECHA: El texto en web es '{fechaTexto}'. Error: {ex.Message}");
                resultado = false;
            }

            return resultado;
        }

        public bool CheckListOfReview(List<string[]> expectedReviewItems)
        {
            WaitForBeingVisible(By.Id("ReviewedCars"));
            return CheckBodyTable(expectedReviewItems, By.Id("ReviewedCars"));
        }
    }
}