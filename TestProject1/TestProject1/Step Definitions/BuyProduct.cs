using FinalProject.POMClasses;
using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.Step_Definitions
{
    [Binding]
    public class BuyProductSteps
    {
        private IWebDriver driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private readonly ScenarioContext _scenarioContext;

        public BuyProductSteps(ScenarioContext scenarioContext)
        {
            // Set our version of the web driver to what was passed via
            // scenarioContext.
            _scenarioContext = scenarioContext;
            this.driver = (IWebDriver)_scenarioContext["mydriver"];
            _specFlowOutputHelper = (ISpecFlowOutputHelper)_scenarioContext["outputHelper"];
        }

        [Given(@"I am logged in")]
        public void GivenIHaveLoggedIn()
        {
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();
            // Load in username and password from external file.
            string username = nonDriverHelp.LoadEnvironmentVariable("username");
            string password = nonDriverHelp.LoadEnvironmentVariable("password");

            LoginPagePOM login = new LoginPagePOM(driver, _specFlowOutputHelper);
            login.Login(username, password);

            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.TakeScreenshot("Logged_In");
        }

        [When(@"I add '(.*)' to my cart")]
        public void WhenIAddAnItemToMyCart(string productName)
        {
            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver, _specFlowOutputHelper);
            shop.AddProductToCart(productName);

            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.TakeScreenshot($"{productName}_added_to_cart");
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyAValidCoupon(string coupon)
        {
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);

            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver, _specFlowOutputHelper);
            _specFlowOutputHelper.WriteLine($"Coupon = {coupon}");
            cart.ApplyCoupon(coupon);

            cart.CheckCouponWasAppliedSuccessfully();
        }

        [When(@"I checkout using valid information")]
        public void WhenICheckoutUsingValidInformation()
        {
            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Cart");

            // Consider using site.NavigateUsingNavLink here instead.
            CartPOM cart = new CartPOM(driver, _specFlowOutputHelper);
            cart.GoToCheckout();

            CheckoutPOM checkout = new CheckoutPOM(driver, _specFlowOutputHelper);
            checkout.EnterDetails();
            checkout.PlaceOrder();

            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.TakeScreenshot("checked_out");
        }

        [Then(@"A discount of '(.*)' should be applied")]
        public void ThenTheAppropriateDiscountShouldBeApplied(string strPercentage)
        {
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();
            decimal desiredDiscountPercent = nonDriverHelp.ConvertStringPercentToDecimal(strPercentage);

            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);

            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver, _specFlowOutputHelper);

            decimal discount = cart.GetCouponDiscount();

            // Get price subtotal.
            decimal subtotal = cart.GetSubtotal();

            decimal discountPercent = discount / subtotal;

            //decimal desiredDiscountPercent = Convert.ToDecimal(
            //    help.LoadParameterFromRunsettings("discountPercentage"));

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
                _specFlowOutputHelper.WriteLine($"Discount percentage is not " +
                    $"{desiredDiscountPercent:P2}, it is {discountPercent:P2}.");
            }

            if (discount == desiredDiscount)
            {
                _specFlowOutputHelper.WriteLine("Desired discount matches actual discount");
            }

            decimal shipping = cart.GetShipping();

            // Calculate total based on previously captured values.
            decimal theoreticalTotalActualDiscount = subtotal + shipping - discount;

            // Calculate total based on previously captured subtotal and
            // shipping, but with desiredDiscount instead of captured discount.
            decimal theoreticalTotalDesiredDiscount = subtotal + shipping - desiredDiscount;

            // Get total as displayed on webpage.
            decimal total = cart.GetTotal();

            try
            {
                Assert.That(total, Is.EqualTo(theoreticalTotalActualDiscount));
            }
            catch (AssertionException)
            {
                _specFlowOutputHelper.WriteLine($"Total is £{total}." +
                    $"It should be £{theoreticalTotalActualDiscount}");
            }

            if (total == theoreticalTotalActualDiscount)
            {
                _specFlowOutputHelper.WriteLine("Total matches expected");
            }
            else if (total == theoreticalTotalDesiredDiscount)
            {
                _specFlowOutputHelper.WriteLine("Total does not match expected, but it was" +
                    " calculated correctly from the subtotal, discount, and " +
                    "shipping displayed on the page.");
            }
            else
            {
                _specFlowOutputHelper.WriteLine("Total was calculated incorrectly");
            }

            help.TakeScreenshot("Cart_After_Attempting_Coupon_Application");
        }

        [Then(@"The order number presented should match the order in my account")]
        public void ThenTheOrderNumberPresentedShouldMatchTheOrderInMyAccount()
        {
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            CheckoutPOM checkout = new CheckoutPOM(driver, _specFlowOutputHelper);
            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);

            string orderNumber = checkout.GetOrderNumber();

            help.TakeScreenshot("Order_received_page");

            site.NavigateUsingNavLink("My account");
            site.NavigateUsingNavLink("Orders");
            MyAccountPOM account = new MyAccountPOM(driver, _specFlowOutputHelper);
            string accountOrderNumber = account.GetRecentOrderNumber();

            Assert.That(orderNumber, Is.EqualTo(accountOrderNumber));

            help.TakeScreenshot("My_account_Orders_page");
        }
    }
}
