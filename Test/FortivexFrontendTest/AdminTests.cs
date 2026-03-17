using NUnit.Framework;
using OpenQA.Selenium;

namespace FortivexTests
{
    public class AdminTests : TestBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            driver.Navigate().GoToUrl(BaseUrl);

            wait.Until(d => {
                try
                {
                    d.FindElement(By.CssSelector(".auth-button")).Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            wait.Until(d => {
                try
                {
                    d.FindElement(By.XPath("//button[text()='Bejelentkezés']")).Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            WaitForElement(By.Id("username")).SendKeys("adminuser");
            driver.FindElement(By.Id("password")).SendKeys("admin123");

            wait.Until(d => {
                try
                {
                    d.FindElement(By.CssSelector(".submit-button")).Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            // Megvárjuk hogy az admin oldalra navigáljon
            wait.Until(d => d.Url.Contains("/admin"));

            // Megvárjuk hogy a tábla betöltsön
            WaitForElement(By.CssSelector(".players-table"));
            Thread.Sleep(500);
        }

        [Test]
        public void AdminOldal_OsszesitettStatisztikakMegjelennek()
        {
            var cards = driver.FindElements(By.CssSelector(".overview-card"));
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        [Test]
        public void AdminOldal_JatekosokTablaLatszik()
        {
            var table = WaitForElement(By.CssSelector(".players-table"));
            Assert.That(table.Displayed, Is.True);
        }

        [Test]
        public void AdminOldal_KeresesMukodik()
        {
            var searchInput = WaitForElement(By.CssSelector(".search-input"));
            searchInput.SendKeys("testplayer");

            // Megvárjuk amíg megjelenik legalább egy sor
            wait.Until(d => d.FindElements(By.CssSelector(".players-table tbody tr")).Count > 0);

            var rows = driver.FindElements(By.CssSelector(".players-table tbody tr"));
            Assert.That(rows.Count, Is.GreaterThan(0));
        }

        [Test]
        public void AdminOldal_JatekosReszletekMegnyilnak()
        {
            var viewBtn = WaitForClickable(By.CssSelector(".action-button.view"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", viewBtn);
            Thread.Sleep(500);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            Assert.That(modal.Displayed, Is.True);
        }

        [Test]
        public void AdminOldal_SzerkesztesModalMegnyilik()
        {
            var editBtn = WaitForElement(By.CssSelector(".action-button.edit"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editBtn);
            Thread.Sleep(500);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            Assert.That(modal.Displayed, Is.True);
        }

        [Test]
        public void AdminOldal_TorlesMegse()
        {
            var deleteBtn = WaitForElement(By.CssSelector(".action-button.delete"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", deleteBtn);
            Thread.Sleep(500);

            var cancelBtn = WaitForElement(By.CssSelector(".modal-button.cancel"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cancelBtn);
            Thread.Sleep(500);

            var modals = driver.FindElements(By.CssSelector(".modal-overlay"));
            Assert.That(modals.Count, Is.EqualTo(0));
        }

        [Test]
        public void AdminOldal_FrissitesGombMukodik()
        {
            var refreshBtn = WaitForClickable(By.CssSelector(".refresh-button"));
            refreshBtn.Click();
            var table = WaitForElement(By.CssSelector(".players-table"));
            Assert.That(table.Displayed, Is.True);
        }
    }
}