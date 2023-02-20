﻿using FinalProject.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V107.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

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

            // Wait for discount to be calculated
            MyHelpers help = new MyHelpers(driver);
            help.WaitForElement(By.CssSelector("td[data-title*='Coupon:'] > " +
                        "span.woocommerce-Price-amount"), 2);

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
                By.CssSelector("tr.cart-subtotal > * > *"))
                .Text;

            Console.WriteLine($"Subtotal = {subtotal}");
            // Remove currency symbol from subtotal by removing first char.
            subtotal = subtotal.Substring(1);
            return Convert.ToDecimal(subtotal);
        }

        public decimal GetShipping()
        {
            string shipping = "";

            // Find shipping.
            shipping = driver.FindElement(
                By.CssSelector("td[data-title='Shipping'] > * > *"))
                .Text;

            Console.WriteLine($"Shipping = {shipping.Substring(11)}");

            // Grab the number from the shipping string
            shipping = shipping.Substring(12);

            return Convert.ToDecimal(shipping);
        }

        public decimal GetTotal()
        {
            string total = "";

            // Find total
            total = driver.FindElement(
                By.CssSelector("td[data-title='Total'] > *"))
                .Text;

            Console.WriteLine($"Total = {total}");

            // Remove currency symbol from total by removing first char.
            total = total.Substring(1);
            return Convert.ToDecimal(total);
        }

        public void RemoveItemFromCart()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/cart/";
            MyHelpers help = new MyHelpers(driver);

            Console.WriteLine("Attempt to remove all items from cart");

            // Check if we can find any of the quantitites
            bool elementPresent;
            elementPresent = help.IsElementPresent(
                By.CssSelector("input[type='number']"));

            if (elementPresent == false)
            {
                Console.WriteLine("No items in cart");
                return;
            }

            // Find a list of all of the quantities to loop through later.
            IReadOnlyList<IWebElement> quantities = driver.FindElements
                (By.CssSelector("input[type='number']"));

            foreach (IWebElement element in quantities)
            {
                help.PutStringInInput(element, "0");
            }

            driver.FindElement(By.CssSelector("button[name='update_cart']"))
                .Click();
        }

        public void GoToCheckout()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
