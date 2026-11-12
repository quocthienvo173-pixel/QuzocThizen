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
            // Khởi tạo WebDriverWait với thời gian chờ tường minh tối đa 10 giây
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void TestLoginSuccessful()
        {
            driver.FindElement(By.CssSelector("[data-test='username']")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("[data-test='login-button']")).Click();

            // Chờ tường minh chuyển đến trang sản phẩm
            bool isAtInventory = wait.Until(d => d.Url.Contains("inventory.html"));
            Assert.That(isAtInventory, Is.True, "Đăng nhập thất bại, không vào được trang sản phẩm.");
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

            // 4. Sử dụng WebDriverWait chờ trang giỏ hàng tải xong
            bool isAtCartPage = wait.Until(d => d.Url.Contains("cart.html"));
            Assert.That(isAtCartPage, Is.True, "Không chuyển hướng thành công sang trang giỏ hàng.");

            // Kiểm tra sản phẩm xuất hiện trong giỏ hàng
            IWebElement cartItem = wait.Until(d => d.FindElement(By.CssSelector("[data-test='inventory-item']")));
            Assert.That(cartItem.Displayed, Is.True, "Sản phẩm không xuất hiện trong giỏ hàng.");
        }

        [Test]
        public void TestLoginErrorWithExplicitWait()
        {
            // Tình huống thực tế: Kiểm tra thông báo lỗi xuất hiện khi nhập sai tài khoản (Locked out user)
            driver.FindElement(By.CssSelector("[data-test='username']")).SendKeys("locked_out_user");
            driver.FindElement(By.CssSelector("[data-test='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.CssSelector("[data-test='login-button']")).Click();

            // Chờ tường minh hộp thoại thông báo lỗi xuất hiện (xử lý tình huống thông báo có độ trễ render)
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("[data-test='error']")));

            // Xác thực nội dung thông báo lỗi
            Assert.That(errorMessage.Text, Does.Contain("Epic sadface: Sorry, this user has been locked out."), "Thông báo lỗi không khớp.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
