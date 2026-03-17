using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FortivexTests
{
    public class TestBase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected const string BaseUrl = "https://wondrous-sprinkles-d87a37.netlify.app/";

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless"); // Ha nem akarod látni a böngészőt
            options.AddArgument("--window-size=1920,1080");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        // Segédfüggvény: megvárja amíg egy elem megjelenik
        protected IWebElement WaitForElement(By by)
        {
            return wait.Until(d => d.FindElement(by));
        }

        // Segédfüggvény: megvárja amíg egy elem kattintható
        protected IWebElement WaitForClickable(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
        }
    }
}