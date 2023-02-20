using FinalProject.POMClasses;
using FinalProject.Utils;

namespace FinalProject.TestCases
{
    internal class PurchaseClothingWithDiscount : Utils.BaseTest
    {
        [Test]
        public void TestCase1()
        {
            MyHelpers help = new MyHelpers(driver);

            // Load in username and password from external file.
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            SiteWidePOM siteWide = new SiteWidePOM(driver);
            siteWide.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Sunglasses");

            siteWide.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);
            string coupon = help.LoadParameterFromRunsettings("coupon");
            cart.ApplyCoupon(coupon);

            decimal discount = cart.GetCouponDiscount();

            // Get price subtotal.
            decimal subtotal = cart.GetSubtotal();

            decimal discountPercent = discount / subtotal;

            decimal desiredDiscountPercent = Convert.ToDecimal(
                help.LoadParameterFromRunsettings("discountPercentage")
                );

            // Find absolute desired discount. Must be rounded to 2 d.p.
            decimal desiredDiscount = desiredDiscountPercent * subtotal;
            // Round to 2 d.p. since this is a price.
            desiredDiscount = Math.Round(desiredDiscountPercent, 2);

            // Compare actual discount to desired discount.
            if (discount != desiredDiscount)
            {
                Console.WriteLine($"Discount percentage is not " +
                    $"{desiredDiscountPercent:P2}, it is {discountPercent:P2}.");
            }

            decimal shipping = cart.GetShipping();

            // Calculate total based on previously captured values.
            decimal theoreticalTotal = subtotal + shipping - discount;
            // Get total as displayed on webpage.
            decimal total = cart.GetTotal();

            if (total != theoreticalTotal)
            {
                Console.WriteLine($"Total is £{total}. It should be £{theoreticalTotal}");
            }

            Assert.That(theoreticalTotal, Is.EqualTo(total),
                "Total calculated after coupon & shipping is not correct");

            cart.RemoveItemFromCart();

            login.Logout();
        }
    }
}
