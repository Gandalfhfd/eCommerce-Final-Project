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
            string ariaLabel = $"Add “{productName}” to your cart";
            // need to wait for product button to load
            // Find product
            Console.WriteLine($"Add {productName} to basket");

            try
            {
                driver.FindElement(By.CssSelector
                    ($"a[aria-label=\"Add “{productName}” to your cart\"]"))
                    .Click();
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not add {productName} to cart");
                return false;
            }
            return true;
        }
    }
}
