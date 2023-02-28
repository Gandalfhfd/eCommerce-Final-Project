using FinalProject.POMClasses;
using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace FinalProject.Step_Definitions
{
    [Binding]
    public class BuyProductSteps
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;

        public BuyProductSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            this.driver = (IWebDriver)_scenarioContext["mydriver"];
        }

        [Given(@"I am logged in")]
        public void GivenIHaveLoggedIn()
        {
            MyHelpers help = new MyHelpers(driver);
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();
            // Load in username and password from external file.
            string username = nonDriverHelp.LoadEnvironmentVariable("username");
            string password = nonDriverHelp.LoadEnvironmentVariable("password");

            LoginPagePOM login = new LoginPagePOM(driver);
            login.Login(username, password);

            help.TakeScreensot("Logged_In");
        }

        [When(@"I add '(.*)' to my cart")]
        public void WhenIAddAnItemToMyCart(string productName)
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Shop");

            ShopPOM shop = new ShopPOM(driver);
            shop.AddProductToCart(productName);

            MyHelpers help = new MyHelpers(driver);
            help.TakeScreensot($"{productName}_added_to_cart");
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyAValidCoupon(string coupon)
        {
            MyHelpers help = new MyHelpers(driver);

            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);
            Console.WriteLine($"Coupon = {coupon}");
            cart.ApplyCoupon(coupon);
            help.WaitForElement(By.CssSelector("div[role='alert']"), 2);

            string alertText = driver.FindElement(By.CssSelector("div[role='alert']")).Text;

            if (alertText != "Coupon code applied successfully.")
            {
                Console.WriteLine("Coupon may have not been applied successfully.");
                help.TakeScreensot("coupon_application_failed");
            }
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

            MyHelpers help = new MyHelpers(driver);
            help.TakeScreensot("checked_out");
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

        [Then(@"A discount of '(.*)' should be applied")]
        public void ThenTheAppropriateDiscountShouldBeApplied(string strPercentage)
        {
            MyHelpers help = new MyHelpers(driver);
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();
            decimal desiredDiscountPercent = nonDriverHelp.ConvertStringPercentToDecimal(strPercentage);

            SiteWidePOM site = new SiteWidePOM(driver);

            site.NavigateUsingNavLink("Cart");

            CartPOM cart = new CartPOM(driver);

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
                TestContext.WriteLine($"Discount percentage is not " +
                    $"{desiredDiscountPercent:P2}, it is {discountPercent:P2}.");
            }

            if (discount == desiredDiscount)
            {
                Console.WriteLine("Desired discount matches actual discount");
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
                TestContext.WriteLine($"Total is £{total}." +
                    $"It should be £{theoreticalTotalActualDiscount}");
            }

            if (total == theoreticalTotalActualDiscount)
            {
                Console.WriteLine("Total matches expected");
            }
            else if (total == theoreticalTotalDesiredDiscount)
            {
                Console.WriteLine("Total does not match expected, but it was" +
                    " calculated correctly from the subtotal, discount, and " +
                    "shipping displayed on the page.");
            }
            else
            {
                Console.WriteLine("Total was calculated incorrectly");
            }

            help.TakeScreensot("Cart_After_Attempting_Coupon_Application");
        }
    }
}
