using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Allure
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("ScreenshotTest")]
    [AllureDisplayIgnored]
    public class Tests
    {
        public static IWebDriver driver;
        public static WebDriverWait wait;
        private string url = "https://www.google.com.ua/";
        [OneTimeSetUp]
        public void ClearResultsDir()
        {
            AllureLifecycle.Instance.CleanupResultDirectory();
        }
        [AllureStep("Browser opened")]
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        [AllureStep("Browser closed")]
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test(Description = "Make Screenshot test")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Olha Melnyk")]
        [AllureFeature("Core")]
        [AllureStep("Make screenshot")]
        public void ScreenshotTest()
        {
                driver.FindElement(By.XPath("//input[@name='q']")).SendKeys("butterfly" + Keys.Enter);
                IWebElement imageButton = wait.Until(e => e.FindElement(By.XPath("//a[contains(text(),'Зображення')]")));
                imageButton.Click();

                IList<IWebElement> elements = wait.Until(e => e.FindElements(By.XPath("//img[@class='rg_i Q4LuWd']")));
                IWebElement webElement = elements[5];
                webElement.Click();

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile("screenshot.png", ScreenshotImageFormat.Png);
            
            AllureLifecycle.Instance.AddAttachment("screenshot.png");
            AllureLifecycle.Instance.WrapInStep(() => Assert.IsNotNull(screenshot, "Screenshot created successfully"));
        }
    }
}