using FinalProject.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Navigate to site");
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account";

            // Dismiss a banner that can get in the way
            driver.FindElement(By.CssSelector("a.woocommerce-store-notice__dismiss-link")).Click();
        }

        public void Login(string username, string password)
        {
            // Assume we're on the my-account page.

            Console.WriteLine("Log in");
            NavigateToLoginPage();

            // Let us use the helper library.
            MyHelpers help = new MyHelpers(driver);

            Console.WriteLine("Input username");
            help.PutStringInInput(By.Id("username"), username);

            Console.WriteLine("Input password");
            help.PutStringInInput(By.Id("password"), password);

            Console.WriteLine("Submit login details");
            driver.FindElement(By.CssSelector("button.woocommerce-form-login__submit")).Click();
        }
    }
}
