using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class CartPOM
    {
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        private readonly By _lblCoupon = By.Id("coupon_code");
        private readonly By _btnApplyCoupon = By.CssSelector("button[value='Apply coupon']");
        private readonly By _lblDiscount = By.CssSelector("td[data-title*='Coupon:'] " +
            "> span.woocommerce-Price-amount");
        private readonly By _lblSubtotal = By.CssSelector("tr.cart-subtotal > * > *");
        private readonly By _lblShipping = By.CssSelector("td[data-title='Shipping'] > * > *");
        private readonly By _lblTotal = By.CssSelector("td[data-title='Total'] > *");
        private readonly By _spnQuantity = By.CssSelector("input[type='number']");
        private readonly By _btnUpdateCart = By.CssSelector("button[name='update_cart']");
        private readonly By _dlgCouponAlert = By.CssSelector("[role='alert']");

        public CartPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public void ApplyCoupon(string coupon)
        {
            _specFlowOutputHelper.WriteLine("Enter coupon");
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            help.PutStringInInput(_lblCoupon, coupon);

            _specFlowOutputHelper.WriteLine("Apply coupon");
            _driver.FindElement(_btnApplyCoupon).Click();
        }

        public void CheckCouponWasAppliedSuccessfully()
        {
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            help.WaitForElement(_dlgCouponAlert, 2);

            string alertText = _driver.FindElement(_dlgCouponAlert).Text;

            if (alertText != "Coupon code applied successfully.")
            {
                _specFlowOutputHelper.WriteLine("Coupon may have not been applied successfully.");

                help.TakeScreenshot("coupon_application_failed");
            }
        }

        public decimal GetCouponDiscount()
        {
            string discountAmount;

            // Wait for discount to be calculated
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            help.WaitForElement(_lblDiscount, 2);

            // Find discount amount in format <£a.b>.
            discountAmount = _driver.FindElement(_lblDiscount).Text;

            _specFlowOutputHelper.WriteLine($"{discountAmount} of discount was applied");
            // Remove currency symbol from discountAmount by removing first char.
            discountAmount = discountAmount.Substring(1);

            return Convert.ToDecimal(discountAmount);
        }

        public decimal GetSubtotal()
        {
            string subtotal;

            // Find subtotal in format <£a.b>.
            subtotal = _driver.FindElement(_lblSubtotal).Text;

            _specFlowOutputHelper.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public decimal GetShipping()
        {
            string shipping;

            // Find shipping.
            shipping = _driver.FindElement(_lblShipping).Text;

            _specFlowOutputHelper.WriteLine($"Shipping = {shipping.Substring(11)}");

            // Grab the number from the shipping string
            shipping = shipping.Substring(12);

            return Convert.ToDecimal(shipping);
        }

        public decimal GetTotal()
        {
            string total;

            // Find total
            total = _driver.FindElement(_lblTotal).Text;

            _specFlowOutputHelper.WriteLine($"Total = {total}");

            // Remove currency symbol from total by removing first char.
            total = total.Substring(1);
            return Convert.ToDecimal(total);
        }

        public void RemoveItemsFromCart()
        {
            _driver.Url = "https://www.edgewordstraining.co.uk/demo-site/cart/";
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);

            _specFlowOutputHelper.WriteLine("Attempt to remove all items from cart");

            // Check if we can find any of the quantitites
            bool elementPresent;
            elementPresent = help.IsElementPresent(_spnQuantity, "spnQuantity");

            if (elementPresent == false)
            {
                _specFlowOutputHelper.WriteLine("No items in cart");
                return;
            }

            // Find a list of all of the quantities to loop through later.
            IReadOnlyList<IWebElement> quantities = _driver.FindElements
                (_spnQuantity);

            foreach (IWebElement element in quantities)
            {
                MyHelpers.PutStringInInput(element, "0");
            }

            _driver.FindElement(_btnUpdateCart).Click();
        }

        public void GoToCheckout()
        {
            SiteWidePOM site = new SiteWidePOM(_driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
