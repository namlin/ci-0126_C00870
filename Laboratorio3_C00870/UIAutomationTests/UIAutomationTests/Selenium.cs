using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace UIAutomationTests
{
    public class Selenium
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup() {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();

            // Esperas explícitas necesarias para Vue:
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Enter_To_List_Of_Countries_Test()
        {
            var url = "http://localhost:8080/";

            _driver.Manage().Window.Maximize();

            _driver.Navigate().GoToUrl(url);

            Assert.IsNotNull(_driver);
        }

        [Test]
        public void Create_Country_End_To_End_Test()
        {
            var url = "http://localhost:8080/";
            _driver.Navigate().GoToUrl(url);

            // Esperar a que aparezca el botón "Agregar País":
            var addButton = _wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(By.XPath("//button[contains(text(),'Agregar País')]"));
                    return element.Displayed ? element : null;
                }

                catch
                {
                    return null;
                }
            });

            addButton.Click();

            _wait.Until(driver => driver.Url.Contains("localhost:8080"));
            Assert.IsTrue(_driver.Url.Contains("localhost:8080"), "No volvió a la lista luego de guardar.");

            // Llenar el formulario con información de un país:
            _driver.FindElement(By.Id("name")).SendKeys("Islandia");

            var selectContinent = _driver.FindElement(By.Id("continente"));
            selectContinent.Click();
            selectContinent.SendKeys("Europa");

            _driver.FindElement(By.Id("idioma")).SendKeys("Islandés");

            // Guardar los datos registrados:
            _driver.FindElement(By.XPath("//button[contains(text(),'Guardar')]")).Click();

            // Esperar redirección a home:
            _wait.Until(driver => driver.Url == "http://localhost:8080/");
            Assert.AreEqual("http://localhost:8080/", _driver.Url);

            // Esperar que la tabla actualizada esté disponible:
            _wait.Until(driver =>
            {
                try
                {
                    var table = driver.FindElement(By.TagName("table"));
                    return table.Displayed ? table : null;
                }

                catch
                {
                    return null;
                }
            });

            // Verificar que el país aparece en el body:
            var bodyText = _driver.FindElement(By.TagName("body")).Text;
            Assert.IsTrue(bodyText.Contains("Islandia"), "El país no aparece en la lista.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
            _driver = null;
        }
    }
}
