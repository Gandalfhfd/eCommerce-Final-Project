using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class LoginPagePOM
    {
        // This is an instance class - it will need to be instantiated before use

        private IWebDriver driver;

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
                driver.FindElement(By.CssSelector("a.woocommerce-store-notice__dismiss-link")).Click();
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
            help.PutStringInInput(By.Id("username"), username);

            TestContext.WriteLine("Input password");
            help.PutStringInInput(By.Id("password"), password);

            TestContext.WriteLine("Submit login details");
            driver.FindElement(By.CssSelector("button.woocommerce-form-login__submit")).Click();
        }

        public void Logout()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("My account");
            TestContext.WriteLine("Logout");
            driver.FindElement(By.LinkText("Logout")).Click();
        }
    }
}
