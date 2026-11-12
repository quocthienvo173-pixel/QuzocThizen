using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SauceDemoAutomation
{
    public class LoginAndCartTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            // Khởi tạo WebDriverWait với thời gian chờ tối đa 10 giây
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void TestLoginSuccessful()
        {
            driver.FindElement(By.CssSelector("[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("[data-test='login-button']")).Click();

            Assert.That(driver.Url, Does.Contain("inventory.html"), "Đăng nhập thất bại, không vào được trang sản phẩm.");
        }

        [Test]
        public void TestAddProductToCartAndOpenCart()
        {
            // 1. Đăng nhập
            driver.FindElement(By.CssSelector("[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("[data-test='login-button']")).Click();

            // 2. Thêm sản phẩm vào giỏ hàng
            driver.FindElement(By.CssSelector("[data-test='add-to-cart-sauce-labs-backpack']")).Click();

            // 3. Mở giỏ hàng
            driver.FindElement(By.CssSelector("[data-test='shopping-cart-link']")).Click();

            // 4. Sử dụng WebDriverWait để chờ trang giỏ hàng tải xong và xác thực URL hoặc phần tử sản phẩm
            bool isAtCartPage = wait.Until(d => d.Url.Contains("cart.html"));
            Assert.That(isAtCartPage, Is.True, "Không chuyển hướng thành công sang trang giỏ hàng.");

            // Kiểm tra sản phẩm xuất hiện trong giỏ hàng một cách an toàn
            IWebElement cartItem = wait.Until(d => d.FindElement(By.CssSelector("[data-test='inventory-item']")));
            Assert.That(cartItem.Displayed, Is.True, "Sản phẩm không xuất hiện trong giỏ hàng.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
