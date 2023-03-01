using FinalProject.POMClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace FinalProject.Utils
{
    [Binding]
    internal class HooksClass
    {
        public static IWebDriver? driver;
        private readonly ScenarioContext _scenarioContext;

        protected string baseUrl = "baseUrl not found";
        protected string browser = "browser not found";

        public HooksClass(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void SetUp()
        {   
            _scenarioContext["testString"] = "Lorem ipsum";

            MyHelpers help = new MyHelpers(driver);
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();
            browser = nonDriverHelp.LoadEnvironmentVariable("browser");
            baseUrl = nonDriverHelp.LoadEnvironmentVariable("baseUrl");

            TestContext.WriteLine($"Read in browser var from runsettings: {browser}");
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

            // Share the driver with anyone who wants it.
            _scenarioContext["mydriver"] = driver;

            // Load in username and password from external file.
            NonDriverHelpers nonDriverHelpers = new NonDriverHelpers();
            string username = nonDriverHelpers.LoadEnvironmentVariable("username");
            string password = nonDriverHelpers.LoadEnvironmentVariable("password");

            // Log in so that we can view the cart.
            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            // Prepare the test by removing all items from the cart
            CartPOM cart = new CartPOM(driver);
            cart.RemoveItemsFromCart();

            login.Logout();

            TestContext.WriteLine("Test start");
        }

        [After]
        public void TearDown()
        {
            TestContext.WriteLine("Test end");

            CartPOM cart = new CartPOM(driver);
            // Try catch ensures driver.Quit() is called.
            try
            {
                cart.RemoveItemsFromCart();
            }
            catch (Exception)
            {
                TestContext.WriteLine("Could not remove items from cart");
            }

            LoginPagePOM login = new LoginPagePOM(driver);
            // Try catch ensures driver.Quit() is called.
            try
            {
                login.Logout();

            }
            catch (Exception)
            {
                TestContext.WriteLine("Could not log out");
            }

            driver.Quit();
        }
    }
}
