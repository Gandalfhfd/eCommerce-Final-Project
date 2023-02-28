using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class CartPOM
    {
        private IWebDriver driver;

        private By lblCoupon = By.Id("coupon_code");
        private By btnApplyCoupon = By.CssSelector("button[value='Apply coupon']");
        private By lblDiscount = By.CssSelector("td[data-title*='Coupon:'] " +
            "> span.woocommerce-Price-amount");
        private By lblSubtotal = By.CssSelector("tr.cart-subtotal > * > *");
        private By lblShipping = By.CssSelector("td[data-title='Shipping'] > * > *");
        private By lblTotal = By.CssSelector("td[data-title='Total'] > *");
        private By spnQuantity = By.CssSelector("input[type='number']");
        private By btnUpdateCart = By.CssSelector("button[name='update_cart']");

        public CartPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void ApplyCoupon(string coupon)
        {
            TestContext.WriteLine("Enter coupon");
            MyHelpers help = new MyHelpers(driver);
            help.PutStringInInput(lblCoupon, coupon);

            TestContext.WriteLine("Apply coupon");
            driver.FindElement(btnApplyCoupon).Click();
        }

        public decimal GetCouponDiscount()
        {
            string discountAmount = "";

            // Wait for discount to be calculated
            MyHelpers help = new MyHelpers(driver);
            help.WaitForElement(lblDiscount, 2);

            // Find discount amount in format <£a.b>.
            discountAmount = driver.FindElement(lblDiscount).Text;

            TestContext.WriteLine($"{discountAmount} of discount was applied");
            // Remove currency symbol from discountAmount by removing first char.
            discountAmount = discountAmount.Substring(1);

            return Convert.ToDecimal(discountAmount);
        }

        public decimal GetSubtotal()
        {
            string subtotal = "";

            // Find subtotal in format <£a.b>.
            subtotal = driver.FindElement(lblSubtotal).Text;

            TestContext.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public decimal GetShipping()
        {
            string shipping = "";

            // Find shipping.
            shipping = driver.FindElement(lblShipping).Text;

            TestContext.WriteLine($"Shipping = {shipping.Substring(11)}");

            // Grab the number from the shipping string
            shipping = shipping.Substring(12);

            return Convert.ToDecimal(shipping);
        }

        public decimal GetTotal()
        {
            string total = "";

            // Find total
            total = driver.FindElement(lblTotal).Text;

            TestContext.WriteLine($"Total = {total}");

            // Remove currency symbol from total by removing first char.
            total = total.Substring(1);
            return Convert.ToDecimal(total);
        }

        public void RemoveItemsFromCart()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/cart/";
            MyHelpers help = new MyHelpers(driver);

            TestContext.WriteLine("Attempt to remove all items from cart");

            // Check if we can find any of the quantitites
            bool elementPresent;
            elementPresent = help.IsElementPresent(spnQuantity);

            if (elementPresent == false)
            {
                TestContext.WriteLine("No items in cart");
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
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
