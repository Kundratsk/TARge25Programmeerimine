using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TARge25shop.KindergartenSeleniumTests
{
    public class KindergartenUITests: IDisposable
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
        // CRUD testid
        // ------------------------------------------------------------------

        /// <summary>
        /// Create test: avab Create vormi, täidab kõik neli välja, salvestab ja kontrollib,
        /// et Index tabelisse tekkis uus rida, kus Group Name, Kindergarten Name,
        /// Children Count ja Teacher Name on täpselt samad, mis sisestati,
        /// ning Created Date ja Updated Date on täidetud.
        /// </summary>
        [Fact]
        public void Create_ShouldAddNewKindergartenToIndex()
        {
            var data = NewData();

            CreateViaUi(data);

            Assert.True(RowExists(data.GroupName), "Uut objekti ei leitud Index tabelist.");

            var cells = GetRow(data.GroupName).FindElements(By.TagName("td"));
            Assert.Equal(data.GroupName, cells[1].Text.Trim());
            Assert.Equal(data.KindergartenName, cells[2].Text.Trim());
            Assert.Equal(data.ChildrenCount.ToString(), cells[3].Text.Trim());
            Assert.Equal(data.TeacherName, cells[4].Text.Trim());
            Assert.NotEmpty(cells[5].Text.Trim());
            Assert.NotEmpty(cells[6].Text.Trim());
        }

        
        /// <summary>
        /// Details test: loob objekti, avab Index tabelis selle rea Details lehe
        /// ja kontrollib, et lehe pealkiri on "Kindergarten Details" ning kõik
        /// väljad (grupp, lasteaed, laste arv, õpetaja) kattuvad sisestatuga
        /// ja Created At ning Updated At on täidetud.
        /// </summary>
        [Fact]
        public void Details_ShouldShowAllFieldsOfCreatedKindergarten()
        {
            var data = NewData();
            CreateViaUi(data);

            OpenDetails(data.GroupName);

            Assert.Equal("Kindergarten Details", _driver.FindElement(By.TagName("h1")).Text.Trim());
            AssertDetailsValues(data);
        }

        /// <summary>
        /// Update test: loob objekti, avab selle Update lehe, muudab kõiki nelja välja,
        /// salvestab ja kontrollib, et Index tabelis on uute väärtustega rida,
        /// vana grupinime enam ei ole ning Details leht näitab uusi väärtusi.
        /// </summary>
        [Fact]
        public void Update_ShouldSaveChangedValues()
        {
            var oldData = NewData();
            var newData = NewData() with { ChildrenCount = 35 };
            CreateViaUi(oldData);

            GetRow(oldData.GroupName).FindElement(By.LinkText("Update")).Click();
            WaitForUrlContains("/Kindergarten/Update");
            FillForm(newData);
            Submit();
            WaitForIndex();

            Assert.True(RowExists(newData.GroupName), "Uut grupinime ei leitud Index tabelist.");
            Assert.False(RowExists(oldData.GroupName), "Vana grupinimi on endiselt Index tabelis.");

            OpenDetails(newData.GroupName);
            AssertDetailsValues(newData);
        }

        /// <summary>
        /// Delete test: loob objekti, avab selle Delete lehe, kontrollib et kuvatud andmed
        /// on õige objekti omad, kinnitab kustutamise ja kontrollib, et rida
        /// on Index tabelist kadunud.
        /// </summary>
        [Fact]
        public void Delete_ShouldRemoveKindergartenFromIndex()
        {
            var data = NewData();
            CreateViaUi(data);

            GetRow(data.GroupName).FindElement(By.LinkText("Delete")).Click();
            WaitForUrlContains("/Kindergarten/Delete");

            Assert.Equal("Delete Kindergarten", _driver.FindElement(By.TagName("h1")).Text.Trim());
            var values = ReadDetailsValues();
            Assert.Equal(data.GroupName, values[0]);
            Assert.Equal(data.KindergartenName, values[1]);
            Assert.Equal(data.ChildrenCount.ToString(), values[2]);
            Assert.Equal(data.TeacherName, values[3]);

            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();
            WaitForIndex();

            Assert.False(RowExists(data.GroupName), "Kustutatud objekt on endiselt Index tabelis.");
        }

        /// <summary>
        /// Terve elutsükli test ühe ja sama objektiga: loob objekti, vaatab Details lehte
        /// (jätab objekti Id meelde), muudab objekti, vaatab uuesti Details lehte
        /// ja kontrollib, et Id on sama ning väärtused uued, ning lõpuks kustutab
        /// objekti ja kontrollib, et seda enam ei ole.
        /// </summary>
        [Fact]
        public void FullCrudFlow_ShouldWorkOnSameObject()
        {
            var created = NewData();
            var updated = NewData() with { ChildrenCount = 28 };

            CreateViaUi(created);
            OpenDetails(created.GroupName);
            AssertDetailsValues(created);
            var idAfterCreate = GetIdFromUrl();

            _driver.FindElement(By.LinkText("Update")).Click();
            WaitForUrlContains("/Kindergarten/Update");
            FillForm(updated);
            Submit();
            WaitForIndex();

            OpenDetails(updated.GroupName);
            AssertDetailsValues(updated);
            Assert.Equal(idAfterCreate, GetIdFromUrl());

            GoToIndex();
            GetRow(updated.GroupName).FindElement(By.LinkText("Delete")).Click();
            WaitForUrlContains("/Kindergarten/Delete");
            Assert.Equal(idAfterCreate, GetIdFromUrl());
            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();
            WaitForIndex();

            Assert.False(RowExists(updated.GroupName), "Objekt ei kustunud.");
            Assert.False(RowExists(created.GroupName), "Vana nimega objekt ilmus tagasi.");
        }

        // ------------------------------------------------------------------
        // Abimeetodid
        // ------------------------------------------------------------------

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
            return _driver.FindElement(RowLocator(groupName));
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

        private List<string> ReadDetailsValues()
        {
            return _driver
                .FindElements(By.CssSelector("dl dd"))
                .Select(e => e.Text.Trim())
                .ToList();
        }

        private void AssertDetailsValues(KindergartenData data)
        {
            var values = ReadDetailsValues();

            Assert.Equal(data.GroupName, values[0]);
            Assert.Equal(data.KindergartenName, values[1]);
            Assert.Equal(data.ChildrenCount.ToString(), values[2]);
            Assert.Equal(data.TeacherName, values[3]);
            Assert.NotEmpty(values[4]);
            Assert.NotEmpty(values[5]);
        }
        private string GetIdFromUrl()
        {
            return new Uri(_driver.Url).Segments.Last().TrimEnd('/');
        }
    }
}
