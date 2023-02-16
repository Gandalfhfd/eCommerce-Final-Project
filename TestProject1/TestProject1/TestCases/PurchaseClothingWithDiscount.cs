using FinalProject.POMClasses;

namespace FinalProject.TestCases
{
    internal class PurchaseClothingWithDiscount : Utils.BaseTest
    {
        [Test]
        public void Lorem()
        {
            // Load in username and password from external file
            string username = TestContext.Parameters["username"];
            string password = TestContext.Parameters["password"];

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            SiteWidePOM siteWide = new SiteWidePOM(driver);
            siteWide.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Belt");

        }
    }
}
