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

        public void AddProductToCart(string productName)
        {
            string ariaLabel = $"Add “{productName}” to your cart";
            // need to wait for product button to load
            // Find product
            Console.WriteLine($"Add {productName} to basket");
            driver.FindElement(By.CssSelector("[data-product_id='31']")).Click();
            // add to cart
        }
    }
}
