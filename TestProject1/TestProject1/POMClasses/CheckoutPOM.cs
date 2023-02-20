using FinalProject.Classes;
using FinalProject.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.POMClasses
{
    internal class CheckoutPOM
    {
        private IWebDriver driver;
        public CheckoutPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void EnterDetails()
        {
            // Instantiating grabs customer details from runsettings file.
            Customer customer = new Customer(driver);

            MyHelpers help = new MyHelpers(driver);

            Console.WriteLine("Enter customer details");

            help.PutStringInInput(By.Id("billing_postcode"), customer.postcode);
            help.PutStringInInput(By.Id("billing_first_name"), customer.firstName);
            help.PutStringInInput(By.Id("billing_last_name"), customer.lastName);
            help.PutStringInInput(By.Id("billing_address_1"), customer.streetAddress);
            help.PutStringInInput(By.Id("billing_city"), customer.townCity);
            help.PutStringInInput(By.Id("billing_postcode"), customer.postcode);
            help.PutStringInInput(By.Id("billing_phone"), customer.phoneNumber);
        }

        public void PlaceOrder()
        {
            MyHelpers helpers= new MyHelpers(driver);
            IWebElement orderButton;

            // Currently WaitForStaleElement is hardly better than a Thread.Sleep.
            // This must change. Remove this comment when it does.
            orderButton = helpers.WaitForStaleElement(By.Id("place_order"), 8, 1000);

            Console.WriteLine("Place order");
            orderButton.Click();
            Thread.Sleep(4000);
        }

        public string GetOrderNumber()
        {
            string orderNumber = driver.FindElement(By.CssSelector("li.order"))
                .Text;

            // Cut stuff that isn't the order number off.
            // This is likely to be brittle.
            orderNumber = orderNumber.Substring(15);

            Console.WriteLine($"Order number = {orderNumber}");
            return orderNumber;
        }
    }
}
