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
    }
