using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.Shared
{
    public class SelectCochesParaReseñar_PO
    {
        protected IWebDriver _driver;
        protected readonly ITestOutputHelper _output;

        // --- LOCALIZADORES (IDs de tu Razor) ---
        private By _inputClaseCoche = By.Id("inputClaseCoche");
        private By _inputColor = By.Id("inputColor");
        private By _buttonSearchCars = By.Id("searchCars");
        private By _buttonWriteReviews = By.Id("purchaseCarButton"); // Botón "Escribir Reseñas"
        private By _tableOfCars = By.Id("TableOfCars");

        // Localizadores de mensajes/modales
        private By _modalTitle = By.ClassName("modal-title");
        private By _modalBody = By.ClassName("modal-body");
        private By _okModalDialog = By.Id("Button_DialogOK");

        public SelectCochesParaReseñar_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            this._output = output;
        }

        // --- LÓGICA DE NEGOCIO ---

        public void SearchCars(string filterClase, string filterColor)
        {
            // Esperamos (ignorando si se ponen 'stale')
            WaitForBeingClickable(_inputClaseCoche);
            WaitForBeingClickable(_inputColor);

            // Buscamos y limpiamos
            var inputClase = _driver.FindElement(_inputClaseCoche);
            inputClase.Clear();
            inputClase.SendKeys(filterClase);

            var inputColor = _driver.FindElement(_inputColor);
            inputColor.Clear();
            inputColor.SendKeys(filterColor);

            // Click en Buscar
            WaitForBeingClickable(_buttonSearchCars);
            _driver.FindElement(_buttonSearchCars).Click();
            System.Threading.Thread.Sleep(500);
        }

        public bool CheckListOfCars(List<string[]> expectedCars)
        {
            return CheckBodyTable(expectedCars, _tableOfCars);
        }

        public void AddCarToReviewList(string carId)
        {
            var buttonId = By.Id("carToReview_" + carId);
            WaitForBeingClickable(buttonId);
            _driver.FindElement(buttonId).Click();
        }

        public void RemoveCarFromReviewList(string carId)
        {
            var buttonId = By.Id("removeCar_" + carId);
            WaitForBeingClickable(buttonId);
            _driver.FindElement(buttonId).Click();
        }

        public void ClickWriteReviews()
        {
            WaitForBeingClickable(_buttonWriteReviews);
            _driver.FindElement(_buttonWriteReviews).Click();
        }

        public bool WriteReviewsNotAvailable()
        {
            try
            {
                var element = _driver.FindElement(_buttonWriteReviews);
                return !element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        // --- MÉTODOS HELPER ROBUSTOS (ANTI-STALE) ---

        public void WaitForBeingClickable(By IdElement)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            // ESTA LÍNEA ES LA MAGIA: Si el elemento caduca, lo vuelve a buscar sin fallar
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(ExpectedConditions.ElementToBeClickable(IdElement));
        }

        public void WaitForBeingVisible(By IdElement)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(ExpectedConditions.ElementIsVisible(IdElement));
        }

        // Resto de helpers originales
        public void InputDateInDatePicker(By datepicker, DateTime date)
        {
            IWebElement webElement = _driver.FindElement(datepicker);
            var action = new Actions(_driver);
            webElement.Clear();
            webElement.Click();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.SendKeys(date.ToString("dd")).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("MM")).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("yyyy")).Perform();
        }

        public bool CheckBodyTable(List<string[]> expectedRows, By IdTable)
        {
            string expectedRow, actualRow;
            int i, j;
            bool result = true;

            WaitForBeingVisible(IdTable); // Usamos la versión robusta

            IList<IWebElement> actualrows = _driver
                .FindElement(IdTable)
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .ToList();

            if (actualrows.Count != expectedRows.Count)
            {
                _output.WriteLine($"Error: \n Expected number of rows:{expectedRows.Count} \n Actual number of rows:{actualrows.Count}");
                return false;
            }

            for (i = 0; i < expectedRows.Count; i++)
            {
                expectedRow = expectedRows[i][0];
                for (j = 1; j < expectedRows[i].Count(); j++)
                    expectedRow = expectedRow + " " + expectedRows[i][j];
                actualRow = actualrows
                    .Select(m => m.Text)
                    .ToList()[i];

                if (!actualRow.StartsWith(expectedRow))
                {
                    _output.WriteLine($"Error: \n \t expected row:{expectedRow} \n \t actual row:{actualRow}");
                    result = false;
                }
            }
            return result;
        }

        public bool CheckModalBodyText(string expectedBody, By modal)
        {
            WaitForBeingVisible(modal);
            var actualBody = _driver.FindElement(_modalBody).Text;
            return actualBody.Contains(expectedBody);
        }

        public bool CheckModalTitleText(string expectedTitle, By modal)
        {
            WaitForBeingVisible(modal);
            var actualTitle = _driver.FindElement(_modalTitle).Text;
            return actualTitle.Contains(expectedTitle);
        }

        public void PressOkModalDialog()
        {
            WaitForBeingVisible(_okModalDialog);
            _driver.FindElement(_okModalDialog).Click();
        }

        public void WaitForBeingVisibleIgnoringExeptionTypes(By IdElement)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 10, 0));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                typeof(WebDriverTimeoutException),
                typeof(UnhandledAlertException),
                typeof(ElementClickInterceptedException),
                typeof(StaleElementReferenceException)); // Añadido Stale aquí también

            bool notFoundButton = true;
            while (notFoundButton)
            {
                try
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(IdElement));
                    notFoundButton = false;
                }
                catch (ElementClickInterceptedException ex)
                {
                    _output.WriteLine(ex.Message);
                }
            }
        }

        public void WaitForTextToBePresentInElement(By IdElement, string expectedText)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            IWebElement element = _driver.FindElement(IdElement);
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, expectedText));
        }

        public void ImplicitWait(int seconds) =>
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
    }
}