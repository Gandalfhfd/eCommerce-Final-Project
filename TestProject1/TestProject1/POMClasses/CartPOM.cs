using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class CartPOM
    {
        private IWebDriver driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        private By lblCoupon = By.Id("coupon_code");
        private By btnApplyCoupon = By.CssSelector("button[value='Apply coupon']");
        private By lblDiscount = By.CssSelector("td[data-title*='Coupon:'] " +
            "> span.woocommerce-Price-amount");
        private By lblSubtotal = By.CssSelector("tr.cart-subtotal > * > *");
        private By lblShipping = By.CssSelector("td[data-title='Shipping'] > * > *");
        private By lblTotal = By.CssSelector("td[data-title='Total'] > *");
        private By spnQuantity = By.CssSelector("input[type='number']");
        private By btnUpdateCart = By.CssSelector("button[name='update_cart']");
        private By dlgCouponAlert = By.CssSelector("[role='alert']");

        public CartPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            this.driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public void ApplyCoupon(string coupon)
        {
            _specFlowOutputHelper.WriteLine("Enter coupon");
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.PutStringInInput(lblCoupon, coupon);

            _specFlowOutputHelper.WriteLine("Apply coupon");
            driver.FindElement(btnApplyCoupon).Click();
        }

        public void CheckCouponWasAppliedSuccessfully()
        {
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.WaitForElement(dlgCouponAlert, 2);

            string alertText = driver.FindElement(dlgCouponAlert).Text;

            if (alertText != "Coupon code applied successfully.")
            {
                _specFlowOutputHelper.WriteLine("Coupon may have not been applied successfully.");

                help.TakeScreenshot("coupon_application_failed");
            }
        }

        public decimal GetCouponDiscount()
        {
            string discountAmount = "";

            // Wait for discount to be calculated
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);
            help.WaitForElement(lblDiscount, 2);

            // Find discount amount in format <£a.b>.
            discountAmount = driver.FindElement(lblDiscount).Text;

            _specFlowOutputHelper.WriteLine($"{discountAmount} of discount was applied");
            // Remove currency symbol from discountAmount by removing first char.
            discountAmount = discountAmount.Substring(1);

            return Convert.ToDecimal(discountAmount);
        }

        public decimal GetSubtotal()
        {
            string subtotal = "";

            // Find subtotal in format <£a.b>.
            subtotal = driver.FindElement(lblSubtotal).Text;

            _specFlowOutputHelper.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public decimal GetShipping()
        {
            string shipping = "";

            // Find shipping.
            shipping = driver.FindElement(lblShipping).Text;

            _specFlowOutputHelper.WriteLine($"Shipping = {shipping.Substring(11)}");

            // Grab the number from the shipping string
            shipping = shipping.Substring(12);

            return Convert.ToDecimal(shipping);
        }

        public decimal GetTotal()
        {
            string total = "";

            // Find total
            total = driver.FindElement(lblTotal).Text;

            _specFlowOutputHelper.WriteLine($"Total = {total}");

            // Remove currency symbol from total by removing first char.
            total = total.Substring(1);
            return Convert.ToDecimal(total);
        }

        public void RemoveItemsFromCart()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/cart/";
            MyHelpers help = new MyHelpers(driver, _specFlowOutputHelper);

            _specFlowOutputHelper.WriteLine("Attempt to remove all items from cart");

            // Check if we can find any of the quantitites
            bool elementPresent;
            elementPresent = help.IsElementPresent(spnQuantity, "spnQuantity");

            if (elementPresent == false)
            {
                _specFlowOutputHelper.WriteLine("No items in cart");
                return;
            }

            // Find a list of all of the quantities to loop through later.
            IReadOnlyList<IWebElement> quantities = driver.FindElements
                (spnQuantity);

            foreach (IWebElement element in quantities)
            {
                help.PutStringInInput(element, "0");
            }

            driver.FindElement(btnUpdateCart).Click();
        }

        public void GoToCheckout()
        {
            SiteWidePOM site = new SiteWidePOM(driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
