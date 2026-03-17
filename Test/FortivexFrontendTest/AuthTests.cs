using NUnit.Framework;
using OpenQA.Selenium;

namespace FortivexTests
{
    public class AuthTests : TestBase
    {
        private void OpenLoginModal()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            WaitForClickable(By.XPath("//button[text()='Bejelentkezés']")).Click();
            WaitForElement(By.CssSelector(".modal-content"));
        }

        private void OpenRegisterModal()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            WaitForClickable(By.CssSelector(".auth-button")).Click();
            WaitForClickable(By.XPath("//button[text()='Regisztráció']")).Click();
            WaitForElement(By.CssSelector(".modal-content"));
        }

        private void Login(string username, string password)
        {
            OpenLoginModal();
            WaitForElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            wait.Until(d => d.Url.Contains("/player") || d.Url.Contains("/admin"));
            Thread.Sleep(500);
        }

        // --- BEJELENTKEZÉS TESZTEK ---

        [Test]
        public void Login_ModalMegnyilik()
        {
            OpenLoginModal();
            Thread.Sleep(500);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            Assert.That(modal.Displayed, Is.True);
        }

        [Test]
        public void Login_ModalBezarodXGombra()
        {
            OpenLoginModal();
            WaitForClickable(By.CssSelector(".close-button")).Click();
            Thread.Sleep(300);
            var modals = driver.FindElements(By.CssSelector(".modal-content"));
            Assert.That(modals.Count, Is.EqualTo(0));
        }

        [Test]
        public void Login_ModalBezarodHatterreKattintasra()
        {
            OpenLoginModal();
            var overlay = WaitForElement(By.CssSelector(".modal-overlay"));
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
                var overlay = arguments[0];
                var rect = overlay.getBoundingClientRect();
                var event = new MouseEvent('click', {
                    clientX: rect.left + 10,
                    clientY: rect.top + 10,
                    bubbles: true
                });
                overlay.dispatchEvent(event);
            ", overlay);
            Thread.Sleep(300);
            var modals = driver.FindElements(By.CssSelector(".modal-content"));
            Assert.That(modals.Count, Is.EqualTo(0));
        }

        [Test]
        public void Login_HelyesAdatokkalSikeres()
        {
            Login("testplayer", "test123");
            var username = WaitForElement(By.CssSelector(".username-link"));
            Assert.That(username.Text, Is.EqualTo("testplayer"));
        }

        [Test]
        public void Login_HelyesAdatokkalAtiranyitPlayerOldalra()
        {
            Login("testplayer", "test123");
            Assert.That(driver.Url, Does.Contain("/player"));
        }

        [Test]
        public void Login_HibásJelszóvalSikertelen()
        {
            OpenLoginModal();
            WaitForElement(By.Id("username")).SendKeys("testplayer");
            driver.FindElement(By.Id("password")).SendKeys("rosszjelszo");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            var error = WaitForElement(By.CssSelector(".error-message"));
            Assert.That(error.Displayed, Is.True);
        }

        [Test]
        public void Login_NemLetezoFelhasznaloval()
        {
            OpenLoginModal();
            WaitForElement(By.Id("username")).SendKeys("nemletezik123");
            driver.FindElement(By.Id("password")).SendKeys("valamijelszo");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            var error = WaitForElement(By.CssSelector(".error-message"));
            Assert.That(error.Displayed, Is.True);
        }

        [Test]
        public void Login_UresMezokkelNemKuld()
        {
            OpenLoginModal();
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            Thread.Sleep(300);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            Assert.That(modal.Displayed, Is.True);
        }

        [Test]
        public void Login_ElfelejtettJelszoLinkMukodik()
        {
            OpenLoginModal();
            driver.FindElement(By.CssSelector(".forgot-password span")).Click();
            wait.Until(d => d.Url.Contains("/forgot-password"));
            Assert.That(driver.Url, Does.Contain("/forgot-password"));
        }

        // --- REGISZTRÁCIÓ TESZTEK ---

        [Test]
        public void Register_ModalMegnyilik()
        {
            OpenRegisterModal();
            Thread.Sleep(500);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", modal);
            Assert.That(modal.Displayed, Is.True);
        }

        [Test]
        public void Register_ModalCimeHelyes()
        {
            OpenRegisterModal();
            Thread.Sleep(500);
            var title = WaitForElement(By.CssSelector(".modal-title"));
            wait.Until(d => d.FindElement(By.CssSelector(".modal-title")).Text.Length > 0);
            Assert.That(title.Text, Is.EqualTo("Regisztráció"));
        }

        [Test]
        public void Register_ModalBezarodXGombra()
        {
            OpenRegisterModal();
            WaitForClickable(By.CssSelector(".close-button")).Click();
            Thread.Sleep(300);
            var modals = driver.FindElements(By.CssSelector(".modal-content"));
            Assert.That(modals.Count, Is.EqualTo(0));
        }

        [Test]
        public void Register_JelszavakNemEgyeznek()
        {
            OpenRegisterModal();
            WaitForElement(By.Id("reg-username")).SendKeys("ujfelhasznalo");
            driver.FindElement(By.Id("reg-email")).SendKeys("teszt@teszt.com");
            driver.FindElement(By.Id("reg-password")).SendKeys("jelszo123");
            driver.FindElement(By.Id("reg-password-confirm")).SendKeys("masjelszo");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            var error = WaitForElement(By.CssSelector(".error-message"));
            Assert.That(error.Text, Does.Contain("nem egyeznek"));
        }

        [Test]
        public void Register_RovidJelszoNemFogadottEl()
        {
            OpenRegisterModal();
            WaitForElement(By.Id("reg-username")).SendKeys("ujfelhasznalo");
            driver.FindElement(By.Id("reg-email")).SendKeys("teszt@teszt.com");
            driver.FindElement(By.Id("reg-password")).SendKeys("abc");
            driver.FindElement(By.Id("reg-password-confirm")).SendKeys("abc");
            driver.FindElement(By.CssSelector(".submit-button")).Click();
            Thread.Sleep(300);
            var modal = WaitForElement(By.CssSelector(".modal-content"));
            Assert.That(modal.Displayed, Is.True);
        }

    }
}