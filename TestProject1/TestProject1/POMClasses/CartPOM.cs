using FinalProject.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V107.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.POMClasses
{
    internal class CartPOM
    {
        // This is an instance class - it will need to be instantiated before use.

        private IWebDriver driver;

        public CartPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void ApplyCoupon(string coupon)
        {
            Console.WriteLine("Enter coupon");
            MyHelpers help = new MyHelpers(driver);
            help.PutStringInInput(By.Id("coupon_code"), coupon);

            Console.WriteLine("Apply coupon");
            driver.FindElement(By.CssSelector("button[value='Apply coupon']"))
                .Click();
        }

        public decimal GetCouponDiscount()
        {
            string discountAmount = "";

            // Find discount amount in format <£a.b>.
            discountAmount = driver.FindElement(
                By.CssSelector("td[data-title*='Coupon:'] > " +
                        "span.woocommerce-Price-amount"))
                .Text;

            Console.WriteLine($"{discountAmount} of discount was applied");
            // Remove currency symbol from discountAmount by removing first char.
            discountAmount = discountAmount.Substring(1);

            return Convert.ToDecimal(discountAmount);
        }

        public decimal GetSubtotal()
        {
            string subtotal = "";

            // Find subtotal in format <£a.b>.
            subtotal = driver.FindElement(
                By.CssSelector("td[data-title='Subtotal'] > " +
                        "span.woocommerce-Price-amount"))
                .Text;

            Console.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public string GetShipping()
        {
            string shipping = "";

            // Find shipping.
            // Need to remove "Flat rate: £". 
            shipping = driver.FindElement(
                By.CssSelector("td[data-title='Shipping'] > * > *"))
                .Text;
            Console.WriteLine($"Shipping = {shipping}");

            return shipping;
            //return Convert.ToDecimal(shipping);
        }
    }
}
