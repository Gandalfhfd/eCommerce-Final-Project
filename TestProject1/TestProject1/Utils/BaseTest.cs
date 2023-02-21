using FinalProject.POMClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace FinalProject.Utils
{
    internal class BaseTest
    {
        protected IWebDriver driver;
        protected string baseUrl;
        protected string browser;

        [SetUp, Ignore("Deprecated")]
        public void SetUp()
        {
            MyHelpers help = new MyHelpers(driver);

            browser = help.LoadParameterFromRunsettings("browser");
            baseUrl = help.LoadParameterFromRunsettings("baseUrl");

            TestContext.WriteLine($"Read in browser var from commandline: {browser}");
            browser = browser.ToLower().Trim();

            switch (browser)
            {
                case "firefox":
                    driver = new FirefoxDriver(); break;
                case "chrome":
                    driver = new ChromeDriver(); break;
                default:
                    TestContext.WriteLine("Unknown browser - starting chrome");
                    driver = new ChromeDriver();
                    break;
            }

            // Load in username and password from external file.
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            // Log in so that we can view the cart.
            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            // Prepare the test by removing all items from the cart
            CartPOM cart = new CartPOM(driver);
            cart.RemoveItemsFromCart();

            login.Logout();

            //TestContext.WriteLine(baseUrl);
            //driver.Url = baseUrl;

            TestContext.WriteLine("Test start");
        }

        [TearDown, Ignore("Deprecated")]
        public void TearDown()
        {
            TestContext.WriteLine("Test end");
            driver.Quit();
        }
    }
}
