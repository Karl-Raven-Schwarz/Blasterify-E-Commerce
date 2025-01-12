using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;

namespace Blasterify.Test.LambdaTest
{
    [TestFixture]
    public class AddMoviesToCart
    {
        private EdgeDriver? driver;
        public IDictionary<string, object>? vars { get; private set; }
        private IJavaScriptExecutor? js;
        [SetUp]
        public void SetUp()
        {
            driver = new EdgeDriver();
            js = (IJavaScriptExecutor) driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver!.Quit();
        }
        [Test]
        public void Test01()
        {
            driver!.Navigate().GoToUrl("https://localhost:44318/Access/LogIn");
            driver.Manage().Window.Size = new System.Drawing.Size(1240, 1047);
            driver.FindElement(By.Id("inputEmail")).Click();
            driver.FindElement(By.Id("inputEmail")).SendKeys("arthas@example.com");
            driver.FindElement(By.Id("inputPassword")).Click();
            driver.FindElement(By.Id("inputPassword")).SendKeys("123456");
            driver.FindElement(By.CssSelector(".btn")).Click();
            driver.FindElement(By.CssSelector(".col:nth-child(3) .text-center:nth-child(2) > .btn")).Click();
            driver.FindElement(By.CssSelector(".col:nth-child(4) .text-center:nth-child(2) > .btn")).Click();
            driver.FindElement(By.CssSelector(".btn-outline-warning")).Click();
            driver.FindElement(By.CssSelector(".card:nth-child(1) > .card-body > .row > .col-sm-2 .btn:nth-child(3) > .svg-inline--fa")).Click();
            driver.FindElement(By.CssSelector(".card:nth-child(1) > .card-body > .row > .col-sm-2 .btn:nth-child(3) > .svg-inline--fa")).Click();
            {
                var element = driver.FindElement(By.CssSelector(".card:nth-child(1) > .card-body > .row > .col-sm-2 .btn:nth-child(3) > .svg-inline--fa"));
                Actions builder = new (driver);
                builder.DoubleClick(element).Perform();
            }
            driver.FindElement(By.CssSelector(".card:nth-child(2) .btn:nth-child(3) > .svg-inline--fa")).Click();
            driver.FindElement(By.Id("inputName")).Click();
            driver.FindElement(By.Id("inputName")).SendKeys("arthas");
            driver.FindElement(By.Id("inputAddress")).Click();
            driver.FindElement(By.Id("inputAddress")).SendKeys("peru");
            driver.FindElement(By.Id("inputCardNumber")).Click();
            driver.FindElement(By.Id("inputCardNumber")).SendKeys("1234567890123456");
            driver.FindElement(By.CssSelector(".btn-success")).Click();
            driver.FindElement(By.CssSelector(".css-1ybfctb")).Click();
            driver.FindElement(By.Id("button-pay")).Click();
            driver.FindElement(By.Name("number")).SendKeys("4507990000000002");
            driver.FindElement(By.Name("expirationDate")).SendKeys("11/28");
            driver.FindElement(By.Name("cvv")).SendKeys("123");
            driver.FindElement(By.Name("cardHolderName")).SendKeys("John Doe");
            driver.FindElement(By.CssSelector(".Yuno-button")).Click();
            driver.FindElement(By.CssSelector("html")).Click();
            Assert.That(driver.FindElement(By.CssSelector("h2")).Text, Is.EqualTo("CompletedRentDetail"));
            driver.FindElement(By.LinkText("My Account")).Click();
            driver.FindElement(By.CssSelector(".btn-danger")).Click();
        }
    }
}
