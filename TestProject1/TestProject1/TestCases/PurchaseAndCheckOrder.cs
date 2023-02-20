using OpenQA.Selenium;
using FinalProject.POMClasses;
using FinalProject.Utils;

namespace FinalProject.TestCases
{
    internal class PurchaseAndCheckOrder : BaseTest
    {
        [Test]
        public void TestCase2()
        {
            MyHelpers help = new MyHelpers(driver);

            // Load in username and password from external file.
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Belt");

            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);
            cart.GoToCheckout();

            // First name
            // Last Name
            // Street address
            // Town/City
            // Postcode
            // Phone
            // Place Order
        }
    }
}
