using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.POMClasses
{
    internal class MyAccountPOM
    {
        private IWebDriver driver;
        public MyAccountPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string GetRecentOrderNumber()
        {
            string orderNumber = driver.FindElement(
                By.CssSelector("tr > .woocommerce-orders-table__cell-order-number > a"))
                .Text;

            Console.WriteLine($"Order number from My account = {orderNumber}");
            return orderNumber.Substring(1);
        }

    }
}
