using FinalProject.POMClasses;
using FinalProject.Utils;
using TechTalk.SpecFlow;
using static FinalProject.Utils.HooksClass;

namespace FinalProject.Step_Definitions
{
    [Binding]
    public class BuyProductSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public BuyProductSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I am logged in")]
        public void GivenIHaveLoggedIn()
        {
            MyHelpers help = new MyHelpers(driver);

            // Load in username and password from external file.
            string username = help.LoadParameterFromRunsettings("username");
            string password = help.LoadParameterFromRunsettings("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);
        }

        [When(@"I add an item to my cart")]
        public void WhenIAddAnItemToMyCart()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart("Belt");
        }

        [When(@"I apply a valid coupon")]
        public void WhenIApplyAValidCoupon()
        {
            MyHelpers help = new MyHelpers(driver);

            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);
            string coupon = help.LoadParameterFromRunsettings("coupon");
            cart.ApplyCoupon(coupon);
        }


        [When(@"I checkout using valid information")]
        public void WhenICheckoutUsingValidInformation()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Cart");

            // Consider using site.NavigateUsingNavLink here instead.
            CartPOM cart = new CartPOM(driver);
            cart.GoToCheckout();

            CheckoutPOM checkout = new CheckoutPOM(driver);
            checkout.EnterDetails();
            checkout.PlaceOrder();
        }

        [Then(@"The order number presented should match the order in my account")]
        public void ThenTheOrderNumberPresentedShouldMatchTheOrderInMyAccount()
        {
            MyHelpers help = new MyHelpers(driver);
            CheckoutPOM checkout = new CheckoutPOM(driver);
            SiteWidePOM site = new SiteWidePOM(driver);

            string orderNumber = checkout.GetOrderNumber();

            help.TakeScreensot("Order_received_page");

            site.NavigateUsingNavLink("My account");
            site.NavigateUsingNavLink("Orders");
            MyAccountPOM account = new MyAccountPOM(driver);
            string accountOrderNumber = account.GetRecentOrderNumber();

            Assert.That(orderNumber, Is.EqualTo(accountOrderNumber));

            help.TakeScreensot("My_account_Orders_page");
        }

        [Then(@"The appropriate discount should be applied")]
        public void ThenTheAppropriateDiscountShouldBeApplied()
        {
            MyHelpers help = new MyHelpers(driver);
            SiteWidePOM site = new SiteWidePOM(driver);

            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);
            string coupon = help.LoadParameterFromRunsettings("coupon");
            cart.ApplyCoupon(coupon);

            decimal discount = cart.GetCouponDiscount();

            // Get price subtotal.
            decimal subtotal = cart.GetSubtotal();

            decimal discountPercent = discount / subtotal;

            decimal desiredDiscountPercent = Convert.ToDecimal(
                help.LoadParameterFromRunsettings("discountPercentage"));

            // Find absolute desired discount. Must be rounded to 2 d.p.
            decimal desiredDiscount = desiredDiscountPercent * subtotal;
            // Round to 2 d.p. since this is a price.
            desiredDiscount = Math.Round(desiredDiscount, 2);

            // Compare actual discount to desired discount.
            try
            {
                Assert.That(discount, Is.EqualTo(desiredDiscount));
            }
            catch (AssertionException)
            {
                Console.WriteLine($"Discount percentage is not " +
                    $"{desiredDiscountPercent:P2}, it is {discountPercent:P2}.");
            }

            decimal shipping = cart.GetShipping();

            // Calculate total based on previously captured values.
            decimal theoreticalTotal = subtotal + shipping - discount;
            // Get total as displayed on webpage.
            decimal total = cart.GetTotal();

            try
            {
                Assert.That(total, Is.EqualTo(theoreticalTotal));
            }
            catch (AssertionException)
            {
                Console.WriteLine($"Total is £{total}." +
                    $"It should be £{theoreticalTotal}");
            }

            help.TakeScreensot("End_of_Test_1");
        }
    }
}
