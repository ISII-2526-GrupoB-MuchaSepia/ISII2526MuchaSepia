
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Reseñar
{
    public class CU_ReseñarCoches_UIT : UC_UIT
    {
        // DATOS DE PRUEBA 1 EXAMEN
        private const string carId1 = "1";
        private const string model1 = "Mercedes Clase C";
        private const string clase1 = "Berlina";
        private const string color1 = "Negro";
        private const string desc1 = "Sedán premium de tamaño medio";

        // DATOS DE PRUEBA 2 EXAMEN
        private const string carId2 = "2";
        private const string model2 = "Volkswagen Golf";
        private const string clase2 = "Compacto Urbano";
        private const string color2 = "Azul";
        private const string desc2 = "Compacto alemán muy vendido";

        // USUARIO REAL
        private const string usuarioReal = "lucas.mdn";
        private const string pais = "España";
        private const string tipoConductor = "Titular";

        // Datos válidos 
        private const string descripcionValida = "Reseña para Volkswagen buena examen"; //PARA EXAMEN
        private const int calificacionValida = 5;

        // --- PAGE OBJECTS ---
        private SelectCochesParaReseñar_PO _selectCochesPO;
        private CreacionesReseñar_PO _crearReseñaPO;
        private DetailReseñar_PO _detalleReseñaPO;

        public CU_ReseñarCoches_UIT(ITestOutputHelper output) : base(output)
        {
            _selectCochesPO = new SelectCochesParaReseñar_PO(_driver, _output);
            _crearReseñaPO = new CreacionesReseñar_PO(_driver, _output);
            _detalleReseñaPO = new DetailReseñar_PO(_driver, _output);
        }

        private void InitialStepsForReviewCars()
        {
            _driver.Navigate().GoToUrl(_URI + "reseñar/select");
        }

        // --- TESTS ---

        [Theory]
        [InlineData(model1, clase1, color1, desc1, "Berlina", "Negro")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_Filtering(string model, string clase, string color, string desc, string filterClase, string filterColor)
        {
            InitialStepsForReviewCars();
            var expectedCars = new List<string[]> {
                new string[] { model, clase, color, desc }
            };

            _selectCochesPO.SearchCars(filterClase, filterColor);
            Assert.True(_selectCochesPO.CheckListOfCars(expectedCars));
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_FlujoCompleto_Examen()
        {

            InitialStepsForReviewCars();

            var expectedReviewItems = new List <string[]> {
                 new string[] {clase2, calificacionValida.ToString(), descripcionValida }
               };
            

        // Act -> Selección
        _selectCochesPO.SearchCars(clase1, "");
            Thread.Sleep(1000);
            _selectCochesPO.AddCarToReviewList(carId1);
            Thread.Sleep(1000);
            _selectCochesPO.SearchCars("", ""); //limpieza filtro
            Thread.Sleep(1000);
            _selectCochesPO.SearchCars("", color2);
            Thread.Sleep(1000);
            _selectCochesPO.AddCarToReviewList(carId2);
            Thread.Sleep(1000);
            _selectCochesPO.RemoveCarFromReviewList(carId1);
            Thread.Sleep(1000);

            _selectCochesPO.ClickWriteReviews();

            // Act -> Rellenar formulario
            _crearReseñaPO.RellenarDatosReseña(usuarioReal, pais, tipoConductor);
            Thread.Sleep(1000);
            _crearReseñaPO.RellenarDatosCoche(descripcionValida, calificacionValida, carId2);
            Thread.Sleep(1000);

            _crearReseñaPO.PulsarPublicarReseñas();

            // Assert -> Verificar página de detalles
            Assert.True(_detalleReseñaPO.CheckReviewDetail(usuarioReal, pais, tipoConductor, DateTime.Now));
            Thread.Sleep(1000);
            Assert.True(_detalleReseñaPO.CheckListOfReview(expectedReviewItems));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_WriteReviewButtonNotAvailable_WhenCartEmpty()
        {
            InitialStepsForReviewCars();
            _selectCochesPO.SearchCars(clase1, color1);
            _selectCochesPO.AddCarToReviewList(carId1);
            _selectCochesPO.RemoveCarFromReviewList(carId1);
            Assert.True(_selectCochesPO.WriteReviewsNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_FlujoCompleto_HappyPath()
        {
            
            InitialStepsForReviewCars();

            var expectedReviewItems = new List<string[]> {
                new string[] { clase1, calificacionValida.ToString(), descripcionValida }
            };

            // Act -> Selección
            _selectCochesPO.SearchCars(clase1, color1);
            _selectCochesPO.AddCarToReviewList(carId1);
            _selectCochesPO.ClickWriteReviews();

            // Act -> Rellenar formulario
            _crearReseñaPO.RellenarDatosReseña(usuarioReal, pais, tipoConductor);
            _crearReseñaPO.RellenarDatosCoche(descripcionValida, calificacionValida, carId1);

            _crearReseñaPO.PulsarPublicarReseñas();

            // Assert -> Verificar página de detalles
            Assert.True(_detalleReseñaPO.CheckReviewDetail(usuarioReal, pais, tipoConductor, DateTime.Now));
            Assert.True(_detalleReseñaPO.CheckListOfReview(expectedReviewItems));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_ModificarLista_VolverAtras()
        {
            InitialStepsForReviewCars();

            _selectCochesPO.SearchCars(clase1, color1);
            _selectCochesPO.AddCarToReviewList(carId1);
            _selectCochesPO.ClickWriteReviews();

            _crearReseñaPO.RellenarDatosReseña(usuarioReal, pais, tipoConductor);

            _crearReseñaPO.PulsarModificarCoches();

            _selectCochesPO.RemoveCarFromReviewList(carId1);

            Assert.True(_selectCochesPO.WriteReviewsNotAvailable());
        }

        [Theory]
        // CASO 1: Usuario vacío -> Mensaje Cliente (Blazor)
        [InlineData("", pais, tipoConductor, descripcionValida, 5, "Faltan datos obligatorios.")]

        // CASO 2: País vacío -> Mensaje Cliente (Blazor)
        [InlineData(usuarioReal, "", tipoConductor, descripcionValida, 5, "Faltan datos obligatorios.")]

        // CASO 3: Mala descripción -> Mensaje Servidor
        [InlineData(usuarioReal, pais, tipoConductor, "Mala descripción", 5, "La reseña debe empezar por Reseña para")]

        

        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Reseñar_ValidacionErrores(string user, string p, string conductor, string desc, int rating, string errorEsperado)
        {
            InitialStepsForReviewCars();

            _selectCochesPO.SearchCars(clase1, color1);
            _selectCochesPO.AddCarToReviewList(carId1);
            _selectCochesPO.ClickWriteReviews();

            _crearReseñaPO.RellenarDatosReseña(user, p, conductor);
            _crearReseñaPO.RellenarDatosCoche(desc, rating, carId1);

            _crearReseñaPO.PulsarPublicarReseñas();

            // Assert
            Assert.True(_crearReseñaPO.ComprobarErrorValidación(errorEsperado),
                $"Se esperaba encontrar el texto: '{errorEsperado}' en la página.");
        }
    }
}