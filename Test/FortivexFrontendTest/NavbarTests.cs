using NUnit.Framework;
using OpenQA.Selenium;

namespace FortivexTests
{
    public class NavbarTests : TestBase
    {
        [Test]
        public void Navbar_LogoMegjelenik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            var logo = WaitForElement(By.CssSelector(".logo"));
            Assert.That(logo.Displayed, Is.True);
        }

        [Test]
        public void Navbar_CimMegjelenik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            var title = WaitForElement(By.CssSelector(".game-title"));
            Assert.That(title.Text, Is.EqualTo("FORTIVEX PROJEKT"));
        }

        [Test]
        public void Navbar_MenupontokMegjelennek()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var menuItems = driver.FindElements(By.CssSelector(".desktop-menu .nav-link"));
            Assert.That(menuItems.Count, Is.EqualTo(5));
        }

        [Test]
        public void Navbar_BejelentkezesGombMegjelenik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            var authButton = WaitForElement(By.CssSelector(".auth-button"));
            Assert.That(authButton.Displayed, Is.True);
        }

        [Test]
        public void Navbar_BejelentkezesDropdownMegnyilik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            var dropdown = WaitForElement(By.CssSelector(".dropdown-menu"));
            Assert.That(dropdown.Displayed, Is.True);
        }

        [Test]
        public void Navbar_DropdownKetLehetosegVan()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            var items = driver.FindElements(By.CssSelector(".dropdown-item"));
            Assert.That(items.Count, Is.EqualTo(2));
        }

        [Test]
        public void Navbar_BejelentkezesUtanFelhasznalosnevLatszik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            WaitForClickable(By.XPath("//button[text()='Bejelentkezés']")).Click();
            WaitForElement(By.Id("username")).SendKeys("testplayer");
            driver.FindElement(By.Id("password")).SendKeys("test123");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            wait.Until(d => d.Url.Contains("/player"));
            Thread.Sleep(500);

            // Újra keressük az elemet az újrarenderelés után
            var username = wait.Until(d => {
                try
                {
                    var el = d.FindElement(By.CssSelector(".username-link"));
                    return el.Text.Length > 0 ? el : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
            Assert.That(username.Text, Is.EqualTo("testplayer"));
        }

        [Test]
        public void Navbar_BejelentkezesUtanKijelentkezesGombLatszik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            WaitForClickable(By.XPath("//button[text()='Bejelentkezés']")).Click();
            WaitForElement(By.Id("username")).SendKeys("testplayer");
            driver.FindElement(By.Id("password")).SendKeys("test123");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            wait.Until(d => d.Url.Contains("/player"));
            Thread.Sleep(500);

            var logoutBtn = wait.Until(d => {
                try
                {
                    var el = d.FindElement(By.CssSelector(".logout-button"));
                    return el.Displayed ? el : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
            Assert.That(logoutBtn.Displayed, Is.True);
        }
    }
}