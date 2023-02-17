using FinalProject.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.POMClasses
{
    internal class ShopPOM
    {
        private IWebDriver driver;
        public ShopPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool AddProductToCart(string productName)
        {
            string productSelectorString = $"Add “{productName}” to your cart";
            Console.WriteLine(productSelectorString);

            try
            {
                driver.FindElement(By.CssSelector
                    ($"a[aria-label='{productSelectorString}']"))
                    .Click();
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not add {productName} to cart");
                return false;
            }

            // Wait for product to be added to basket before moving on.
            MyHelpers help = new MyHelpers(driver);
            help.WaitForElement(By.CssSelector("a.added_to_cart"), 2);

            return true;
        }
    }
}
