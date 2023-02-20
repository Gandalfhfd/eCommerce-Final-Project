using FinalProject.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V107.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            // Could simply reduce quantity to 0 on all of them, then
            // Update cart. Would be quicker and less prone to breakage.
            // Need answer on why Try Catch isn't working, regardless.
                        MyHelpers help = new MyHelpers(driver);

            Console.WriteLine("Attempt to remove all items from cart");

            /* Bit of a hack which checks that the page has fully loaded by
             * checking that it has scrolled. Only really makes sense if a
             * coupon has just been added. */

            // ATTENTION TO STEVE
            // The below for loop block is the piece of bad code I was talking
            // about. It's still throwing an exception. Not sure what I want to
            // put into the catch.


            help.WaitForScroll(3);

            // Remove every item that you can.
            // Currently only removes 1 item.
            // Just does not work.

            for (int i = 0; i < 100; i++)
            {
                if (help.IsElementPresent(By.CssSelector("p.cart-empty")))
                {
                    /* An element declaring the cart is empty has apeared, so
                     quit the function. */
                    return;
                }
                else
                {
                    // The cart may not be empty, so keep trying to empty the cart.
                    try
                    {
                        driver.FindElement(By.LinkText("×")).Click();
                        Console.WriteLine($"Removed {i + 1} item from cart");
                    }
                    catch (NoSuchElementException)
                    {
                        return;
                        //throw new NoSuchElementException("blah");
                    }
                }
            }
        }

        public void GoToCheckout()
        {
            SiteWidePOM site = new SiteWidePOM(driver);
            site.NavigateUsingNavLink("Proceed to checkout");
        }
    }
}
