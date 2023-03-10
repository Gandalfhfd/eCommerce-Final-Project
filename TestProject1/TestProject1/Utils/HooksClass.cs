using FinalProject.POMClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.Utils
{
    [Binding]
    internal class HooksClass
    {
        public static IWebDriver? _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private readonly ScenarioContext _scenarioContext;

        protected string _baseUrl = "baseUrl not found";
        protected string _browser = "browser not found";

        public HooksClass(ScenarioContext scenarioContext,
            ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        [Before]
        public void SetUp()
        {
            _browser = NonDriverHelpers.LoadEnvironmentVariable("browser");
            _baseUrl = NonDriverHelpers.LoadEnvironmentVariable("baseUrl");

            _specFlowOutputHelper.WriteLine($"Read in browser var from runsettings: {_browser}");
            _browser = _browser.ToLower().Trim();

            switch (_browser)
            {
                case "firefox":
                    _driver = new FirefoxDriver(); break;
                case "chrome":
                    _driver = new ChromeDriver(); break;
                default:
                    _specFlowOutputHelper.WriteLine("Unknown browser - starting chrome");
                    _driver = new ChromeDriver();
                    break;
            }

            // Share the driver with anyone who wants it.
            _scenarioContext["mydriver"] = _driver;
            // Share the specFlowOutputHelper with anyone who wants it.
            _scenarioContext["outputHelper"] = _specFlowOutputHelper;

            // Load in username and password from external file.
            string username = NonDriverHelpers.LoadEnvironmentVariable("username");
            string password = NonDriverHelpers.LoadEnvironmentVariable("password");

            // Log in so that we can view the cart.
            LoginPagePOM login = new LoginPagePOM(_driver, _specFlowOutputHelper);
            login.Login(username, password);

            // Prepare the test by removing all items from the cart
            CartPOM cart = new CartPOM(_driver, _specFlowOutputHelper);
            cart.RemoveItemsFromCart();

            login.Logout();

            _specFlowOutputHelper.WriteLine("Test start");
        }

        [After]
        public void TearDown()
        {
            _specFlowOutputHelper.WriteLine("Test end");

            CartPOM cart = new CartPOM(_driver, _specFlowOutputHelper);
            // Try catch ensures driver.Quit() is called.
            try
            {
                cart.RemoveItemsFromCart();
            }
            catch (Exception)
            {
                _specFlowOutputHelper.WriteLine("Could not remove items from cart");
            }

            LoginPagePOM login = new LoginPagePOM(_driver, _specFlowOutputHelper);
            // Try catch ensures driver.Quit() is called.
            try
            {
                login.Logout();

            }
            catch (Exception)
            {
                _specFlowOutputHelper.WriteLine("Could not log out");
            }

            _driver.Quit();
        }
    }
}
