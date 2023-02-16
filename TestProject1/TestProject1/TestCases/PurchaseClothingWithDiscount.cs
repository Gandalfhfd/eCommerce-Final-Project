using FinalProject.POMClasses;
using FinalProject.Utils;

namespace FinalProject.TestCases
{
    internal class PurchaseClothingWithDiscount : Utils.BaseTest
    {
        [Test]
        public void Lorem()
        {
            MyHelpers help = new MyHelpers(driver);

            // Load in username and password from external file
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            SiteWidePOM siteWide = new SiteWidePOM(driver);
            siteWide.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Belt");

        }
    }
}
