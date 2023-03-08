using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class LoginPagePOM
    {
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        private readonly By _btnDismissStoreNotice = By.CssSelector("a.woocommerce-store-notice__dismiss-link");
        private readonly By _txtUsername = By.Id("username");
        private readonly By _txtPassword = By.Id("password");
        private readonly By _btnLogin = By.CssSelector("button.woocommerce-form-login__submit");
        private readonly By _btnLogout = By.LinkText("Logout");

        public LoginPagePOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        // Edgewords Shop-specific setup
        public void NavigateToLoginPage()
        {
            _specFlowOutputHelper.WriteLine("Navigate to login page");
            _driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account";

            // Dismiss a banner that can get in the way
            try
            {
                _driver.FindElement(_btnDismissStoreNotice).Click();
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
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);

            _specFlowOutputHelper.WriteLine("Log in");

            _specFlowOutputHelper.WriteLine("Input username");
            help.PutStringInInput(_txtUsername, username);

            _specFlowOutputHelper.WriteLine("Input password");
            help.PutStringInInput(_txtPassword, password);

            _specFlowOutputHelper.WriteLine("Submit login details");
            _driver.FindElement(_btnLogin).Click();
        }

        public void Logout()
        {
            SiteWidePOM site = new SiteWidePOM(_driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("My account");

            _specFlowOutputHelper.WriteLine("Logout");
            _driver.FindElement(_btnLogout).Click();
        }
    }
}
