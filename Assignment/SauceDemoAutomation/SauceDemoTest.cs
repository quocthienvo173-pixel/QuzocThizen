using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemoAutomation
{
    public class SauceDemoTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // Khởi tạo ChromeDriver trực tiếp
            driver = new ChromeDriver(options);
        }

        [Test]
        public void TestOpenSauceDemoAndVerifyTitle()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            string expectedTitle = "Swag Labs";
            Assert.That(driver.Title, Is.EqualTo(expectedTitle));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
