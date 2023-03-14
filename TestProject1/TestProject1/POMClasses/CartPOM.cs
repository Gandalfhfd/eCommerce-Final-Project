using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class CartPOM
    {
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private readonly ScenarioContext _scenarioContext;

        private readonly By _lblCoupon = By.Id("coupon_code");
        private readonly By _btnApplyCoupon = By.CssSelector("button[value='Apply coupon']");
        private IWebElement _btnApplyCouponElmnt => _driver.FindElement(_btnApplyCoupon);
        private readonly By _lblDiscount = By.CssSelector("td[data-title*='Coupon:'] " +
            "> span.woocommerce-Price-amount");
        private IWebElement _lblDiscountElmnt => _driver.FindElement(_lblDiscount);
        private readonly By _lblSubtotal = By.CssSelector("tr.cart-subtotal > * > *");
        private IWebElement _lblSubtotalElmnt => _driver.FindElement(_lblSubtotal);
        private readonly By _lblShipping = By.CssSelector("td[data-title='Shipping'] > * > *");
        private IWebElement _lblShippingElmnt => _driver.FindElement(_lblShipping);
        private readonly By _lblTotal = By.CssSelector("td[data-title='Total'] > *");
        private IWebElement _lblTotalElmnt => _driver.FindElement(_lblTotal);
        private readonly By _spnQuantity = By.CssSelector("input[type='number']");
        // Find a list of all of the quantities to loop through later.
        private IReadOnlyList<IWebElement> _quantities => _driver.FindElements
            (_spnQuantity);
        private readonly By _btnUpdateCart = By.CssSelector("button[name='update_cart']");
        private IWebElement _btnUpdateCartElmnt => _driver.FindElement(_btnUpdateCart);
        private readonly By _dlgCouponAlert = By.CssSelector("[role='alert']");
        private IWebElement _dlgCouponAlertElmnt => _driver.FindElement(_dlgCouponAlert);

        public CartPOM(IWebDriver driver,
            ISpecFlowOutputHelper specFlowOutputHelper,
            ScenarioContext scenarioContext)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
            _scenarioContext = scenarioContext;
        }

        public void ApplyCoupon(string coupon)
        {
            _specFlowOutputHelper.WriteLine("Enter coupon");
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            help.PutStringInInput(_lblCoupon, coupon);

            _specFlowOutputHelper.WriteLine("Apply coupon");
            _btnApplyCouponElmnt.Click();
        }

        public void CheckCouponWasAppliedSuccessfully()
        {
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            help.WaitForElement(_dlgCouponAlert, 2);

            string alertText = _dlgCouponAlertElmnt.Text;

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
            discountAmount = _lblDiscountElmnt.Text;

            _specFlowOutputHelper.WriteLine($"{discountAmount} of discount was applied");
            // Remove currency symbol from discountAmount by removing first char.
            discountAmount = discountAmount.Substring(1);

            return Convert.ToDecimal(discountAmount);
        }

        public decimal GetSubtotal()
        {
            string subtotal;

            // Find subtotal in format <£a.b>.
            subtotal = _lblSubtotalElmnt.Text;

            _specFlowOutputHelper.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public decimal GetShipping()
        {
            string shipping;

            // Find shipping.
            shipping = _lblShippingElmnt.Text;

            _specFlowOutputHelper.WriteLine($"Shipping = {shipping.Substring(11)}");

            // Grab the number from the shipping string
            shipping = shipping.Substring(12);

            return Convert.ToDecimal(shipping);
        }

        public decimal GetTotal()
        {
            string total;

            // Find total
            total = _lblTotalElmnt.Text;

            _specFlowOutputHelper.WriteLine($"Total = {total}");

            // Remove currency symbol from total by removing first char.
            total = total.Substring(1);
            return Convert.ToDecimal(total);
        }

        public void RemoveItemsFromCart()
        {
            _driver.Url = (string)_scenarioContext["baseUrl"] + "cart/";
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

            foreach (IWebElement element in _quantities)
            {
                MyHelpers.PutStringInInput(element, "0");
            }

            _btnUpdateCartElmnt.Click();
        }

        public void GoToCheckout()
        {
            SiteWidePOM site = new SiteWidePOM(_driver, _specFlowOutputHelper);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
