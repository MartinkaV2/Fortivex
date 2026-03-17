using NUnit.Framework;
using OpenQA.Selenium;

namespace FortivexTests
{
    public class PlayerTests : TestBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

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

            WaitForElement(By.Id("username")).SendKeys("testplayer");
            driver.FindElement(By.Id("password")).SendKeys("test123");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            wait.Until(d => d.Url.Contains("/player"));
            Thread.Sleep(1000);
        }

        [Test]
        public void PlayerOldal_UdvozloSzovegMegjelenik()
        {
            var title = WaitForElement(By.CssSelector(".welcome-title"));
            Assert.That(title.Text, Does.Contain("testplayer"));
        }

        [Test]
        public void PlayerOldal_StatisztikakMegjelennek()
        {
            // Megvárjuk hogy legalább egy stat-card megjelenjen
            wait.Until(d => d.FindElements(By.CssSelector(".stat-card")).Count > 0);

            var statCards = driver.FindElements(By.CssSelector(".stat-card"));
            Assert.That(statCards.Count, Is.EqualTo(4));
        }

        [Test]
        public void PlayerOldal_SzintMegjelenik()
        {
            var level = WaitForElement(By.CssSelector(".stat-card.highlight .stat-value"));
            Assert.That(int.Parse(level.Text), Is.GreaterThan(0));
        }

        [Test]
        public void PlayerOldal_PalyaHaladasMegjelenik()
        {
            var mapsCard = WaitForElement(By.CssSelector(".maps-progress-card"));
            Assert.That(mapsCard.Displayed, Is.True);
        }

        [Test]
        public void PlayerOldal_TeljesitmenyekMegjelennek()
        {
            var achievements = WaitForElement(By.CssSelector(".achievements-card"));
            Assert.That(achievements.Displayed, Is.True);
        }

        [Test]
        public void PlayerOldal_UtolsoJatekokMegjelennek()
        {
            var history = WaitForElement(By.CssSelector(".history-card"));
            Assert.That(history.Displayed, Is.True);
        }

        [Test]
        public void PlayerOldal_FrissitesUtanNevMegmarad()
        {
            driver.Navigate().Refresh();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(2000); // auth store visszatöltése miatt
            var title = WaitForElement(By.CssSelector(".welcome-title"));
            Assert.That(title.Text, Does.Contain("testplayer"));
        }

        [Test]
        public void PlayerOldal_FrissitesUtanStatisztikakMegmaradnak()
        {
            var levelElott = WaitForElement(By.CssSelector(".stat-card.highlight .stat-value")).Text;
            driver.Navigate().Refresh();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(2000);
            var levelUtan = WaitForElement(By.CssSelector(".stat-card.highlight .stat-value")).Text;
            Assert.That(levelUtan, Is.EqualTo(levelElott));
        }
    }
}