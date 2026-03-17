using NUnit.Framework;
using OpenQA.Selenium;

namespace FortivexTests
{
    public class DownloadDropdownTests : TestBase
    {
        [Test]
        public void DownloadGomb_Megjelenik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            var btn = WaitForElement(By.CssSelector(".download-dropdown .cta-button.primary"));

            // Scrollozzunk a gombhoz
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(300);

            Assert.That(btn.Displayed, Is.True);
        }

        [Test]
        public void DownloadGomb_SzovegeHelyes()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            var btn = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(300);

            // Text helyett GetAttribute("textContent") mert a span miatt összetett a szöveg
            var text = btn.GetAttribute("textContent").Trim();
            Assert.That(text, Does.Contain("Játék Letöltése"));
        }

        [Test]
        public void DownloadDropdown_Megnyilik()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var btn = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(500);
            var menu = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .dropdown-menu"));
            Assert.That(menu.Displayed, Is.True);
        }

        [Test]
        public void DownloadDropdown_HaromLehetosegVan()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".download-dropdown .cta-button.primary")).Click();
            var items = driver.FindElements(By.CssSelector(".download-dropdown .dropdown-item"));
            Assert.That(items.Count, Is.EqualTo(3));
        }

        [Test]
        public void DownloadDropdown_WindowsLetoltesVan()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            var btn = WaitForClickable(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(300);
            btn.Click();

            var items = driver.FindElements(By.CssSelector(".hero-buttons .download-dropdown .dropdown-item"));
            var text = items.Select(i => i.GetAttribute("textContent").Trim()).ToList();
            Assert.That(text.Any(t => t.Contains("Windows")), Is.True);
        }

        [Test]
        public void DownloadDropdown_LinuxLetoltesVan()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            var btn = WaitForClickable(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(500);

            var items = driver.FindElements(By.CssSelector(".hero-buttons .download-dropdown .dropdown-item"));
            var texts = items.Select(i => i.GetAttribute("textContent").Trim()).ToList();
            Assert.That(texts.Any(t => t.Contains("Linux")), Is.True);
        }

        [Test]
        public void DownloadDropdown_AndroidLetoltesVan()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var btn = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(500);
            var items = driver.FindElements(By.CssSelector(".hero-buttons .download-dropdown .dropdown-item"));
            var texts = items.Select(i => i.GetAttribute("textContent").Trim()).ToList();
            Assert.That(texts.Any(t => t.Contains("Android")), Is.True);
        }

        [Test]
        public void DownloadDropdown_BezarodKulsoKattintasra()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var btn = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(500);
            WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .dropdown-menu"));
            // Scrollozzunk vissza felülre és kattintsunk a navbar logóra
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(300);
            var logo = driver.FindElement(By.CssSelector(".navbar-logo"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", logo);
            Thread.Sleep(500);
            var menus = driver.FindElements(By.CssSelector(".hero-buttons .download-dropdown .dropdown-menu"));
            Assert.That(menus.Count, Is.EqualTo(0));
        }

        [Test]
        public void DownloadDropdown_WindowsLinkHelyes()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            WaitForClickable(By.CssSelector(".download-dropdown .cta-button.primary")).Click();
            var windowsLink = WaitForElement(By.XPath("//a[contains(@href, 'Fortivex_The_Game.zip')]"));
            Assert.That(windowsLink.GetAttribute("href"), Does.Contain("Fortivex_The_Game.zip"));
        }


        [Test]
        public void DownloadDropdown_AndroidLinkHelyes()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var btn = WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .cta-button.primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(500);
            WaitForElement(By.CssSelector(".hero-buttons .download-dropdown .dropdown-menu"));

            // Keressük az összes dropdown itemet és ellenőrizzük az href attribútumot
            var items = driver.FindElements(By.CssSelector(".hero-buttons .download-dropdown .dropdown-item"));
            var androidLink = items.FirstOrDefault(i => {
                var href = i.GetAttribute("href") ?? i.GetAttribute("data-href") ?? "";
                return href.Contains("Fortivex_Mobile.apk");
            });

            // Ha href alapján nem találjuk, szöveg alapján keressük
            if (androidLink == null)
            {
                androidLink = items.FirstOrDefault(i =>
                    i.GetAttribute("textContent")?.Contains("Android") == true);
            }

            Assert.That(androidLink, Is.Not.Null);
        }
    }
}