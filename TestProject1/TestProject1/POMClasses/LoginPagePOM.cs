using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class LoginPagePOM
    {
        private IWebDriver driver;

        private By btnDismissStoreNotice = By.CssSelector("a.woocommerce-store-notice__dismiss-link");
        private By txtUsername = By.Id("username");
        private By txtPassword = By.Id("password");
        private By btnLogin = By.CssSelector("button.woocommerce-form-login__submit");
        private By btnLogout = By.LinkText("Logout");

        public LoginPagePOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        // Edgewords Shop-specific setup
        public void NavigateToLoginPage()
        {
            TestContext.WriteLine("Navigate to login page");
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account";

            // Dismiss a banner that can get in the way
            try
            {
                driver.FindElement(btnDismissStoreNotice).Click();
            }
            catch (ElementNotInteractableException)
            {
                TestContext.WriteLine("Banner already dismissed");
            }
        }

        public void Login(string username, string password)
        {
            NavigateToLoginPage();

            // Let us use the helper library.
            MyHelpers help = new MyHelpers(driver);

            TestContext.WriteLine("Log in");

            TestContext.WriteLine("Input username");
            help.PutStringInInput(txtUsername, username);

            TestContext.WriteLine("Input password");
            help.PutStringInInput(txtPassword, password);

            TestContext.WriteLine("Submit login details");
            driver.FindElement(btnLogin).Click();
        }

        public void Logout()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("My account");
            TestContext.WriteLine("Logout");
            driver.FindElement(btnLogout).Click();
        }
    }
}
