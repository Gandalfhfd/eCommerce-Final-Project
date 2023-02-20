using FinalProject.POMClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace FinalProject.Utils
{
    internal class BaseTest
    {
        // May be a good idea to try catch grabbing parameters from testcontext
        protected IWebDriver driver;
        protected string baseUrl;
        protected string browser;

        [SetUp]
        public void SetUp()
        {
            MyHelpers help = new MyHelpers(driver);

            browser = help.LoadParameterFromRunsettings("browser");
            baseUrl = help.LoadParameterFromRunsettings("baseUrl");

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

            //Console.WriteLine(baseUrl);
            //driver.Url = baseUrl;

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
