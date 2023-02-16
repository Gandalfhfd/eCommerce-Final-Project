using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace FinalProject.Utils
{
    internal class BaseTest
    {
        // May be a good idea to try catch grabbing parameters from testcontext
        protected IWebDriver driver;
        protected string? baseUrl;
        protected string? browser;

        [SetUp]
        public void SetUp()
        {
            browser = TestContext.Parameters["browser"];
            baseUrl = TestContext.Parameters["baseUrl"];
            //driver = TestContext.Parameters

            Console.WriteLine($"Read in browser var from commandline: {browser}");
            browser = browser.ToLower().Trim();

            switch (browser)
            {
                case "firefox":
                    driver = new FirefoxDriver(); break;
                case "chrome":
                    driver = new ChromeDriver(); break;
                default:
                    Console.WriteLine("Unknown browser - starting chrome");
                    driver = new ChromeDriver();
                    break;
            }

            Console.WriteLine(baseUrl);
            driver.Url = baseUrl;
            Console.WriteLine("Test start");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Test end");
            driver.Quit();
        }
    }
}
