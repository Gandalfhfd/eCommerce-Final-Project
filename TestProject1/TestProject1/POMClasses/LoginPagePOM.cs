using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class LoginPagePOM
    {
        private IWebDriver driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        private By btnDismissStoreNotice = By.CssSelector("a.woocommerce-store-notice__dismiss-link");
        private By txtUsername = By.Id("username");
        private By txtPassword = By.Id("password");
        private By btnLogin = By.CssSelector("button.woocommerce-form-login__submit");
        private By btnLogout = By.LinkText("Logout");

        public LoginPagePOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            this.driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        // Edgewords Shop-specific setup
        public void NavigateToLoginPage()
        {
            _specFlowOutputHelper.WriteLine("Navigate to login page");
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account";

            // Dismiss a banner that can get in the way
            try
            {
                driver.FindElement(btnDismissStoreNotice).Click();
            }
            catch (ElementNotInteractableException)
            {
                _specFlowOutputHelper.WriteLine("Banner already dismissed");
            }
        }

        public void Login(string username, string password)
        {
            NavigateToLoginPage();

            // Let us use the helper library.
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);

            _specFlowOutputHelper.WriteLine("Log in");

            _specFlowOutputHelper.WriteLine("Input username");
            help.PutStringInInput(txtUsername, username);

            _specFlowOutputHelper.WriteLine("Input password");
            help.PutStringInInput(txtPassword, password);

            _specFlowOutputHelper.WriteLine("Submit login details");
            driver.FindElement(btnLogin).Click();
        }

        public void Logout()
        {
            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("My account");

            _specFlowOutputHelper.WriteLine("Logout");
            driver.FindElement(btnLogout).Click();
        }
    }
}
