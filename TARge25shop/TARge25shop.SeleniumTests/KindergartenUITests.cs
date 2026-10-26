using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TARge25shop.SeleniumTests
{
    public class KindergartenUITests : IDisposable
    {

        private const string BaseUrl = "https://localhost:7036";

        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private record KindergartenData(
            string GroupName,
            string KindergartenName,
            int ChildrenCount,
            string TeacherName);

        public KindergartenUITests()
        {
            var options = new ChromeOptions();
            options.AcceptInsecureCertificates = true;
            options.AddArgument("--window-size=1400,900");
            // options.AddArgument("--headless=new");

            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }




    // ------------------------------------------------------------------
    // Abimeetodid
    // ---------
    private static KindergartenData NewData()
        {
            var suffix = Guid.NewGuid().ToString("N")[..8];
            return new KindergartenData(
                $"Grupp-{suffix}",
                $"Lasteaed-{suffix}",
                20,
                $"Opetaja-{suffix}");
        }

        private void GoToIndex()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/Kindergarten");
            WaitForIndex();
        }
        private void WaitForIndex()
        {
            _wait.Until(d => d.Url.TrimEnd('/').EndsWith("/Kindergarten", StringComparison.OrdinalIgnoreCase));
        }
        private void WaitForUrlContains(string part)
        {
            _wait.Until(d => d.Url.Contains(part, StringComparison.OrdinalIgnoreCase));
        }

        private void CreateViaUi(KindergartenData data)
        {
            GoToIndex();
            _driver.FindElement(By.LinkText("Create")).Click();
            WaitForUrlContains("/Kindergarten/Create");
            FillForm(data);
            Submit();
            WaitForIndex();
        }
        private void FillForm(KindergartenData data)
        {
            SetField("GroupName", data.GroupName);
            SetField("KindergartenName", data.KindergartenName);
            SetField("ChildrenCount", data.ChildrenCount.ToString());
            SetField("TeacherName", data.TeacherName);
        }
        private void SetField(string id, string value)
        {
            var field = _driver.FindElement(By.Id(id));
            field.Clear();
            field.SendKeys(value);
        }
        private void Submit()
        {
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        private IWebElement GetRow(string groupName)
        {
            var row = _driver.FindElement(RowLocator(groupName));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', behavior: 'instant'});", row);
            return row;
        }

        private bool RowExists(string groupName)
        {
            return _driver.FindElements(RowLocator(groupName)).Count > 0;
        }
        private static By RowLocator(string groupName)
        {
            return By.XPath($"//table//tbody/tr[td[normalize-space(.)='{groupName}']]");
        }
        private void OpenDetails(string groupName)
        {
            if (!_driver.Url.TrimEnd('/').EndsWith("/Kindergarten", StringComparison.OrdinalIgnoreCase))
            {
                GoToIndex();
            }

            GetRow(groupName).FindElement(By.LinkText("Details")).Click();
            WaitForUrlContains("/Kindergarten/Details");
        }
    }
}
