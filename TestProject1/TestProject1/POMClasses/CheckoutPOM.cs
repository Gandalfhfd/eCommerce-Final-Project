﻿using FinalProject.Classes;
using FinalProject.Utils;
using OpenQA.Selenium;

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

            TestContext.WriteLine("Enter customer details");

            help.PutStringInInput(By.Id("billing_first_name"), customer.firstName);
            help.PutStringInInput(By.Id("billing_last_name"), customer.lastName);
            help.PutStringInInput(By.Id("billing_address_1"), customer.streetAddress);
            help.PutStringInInput(By.Id("billing_city"), customer.townCity);
            help.PutStringInInput(By.Id("billing_postcode"), customer.postcode);
            help.PutStringInInput(By.Id("billing_phone"), customer.phoneNumber);
            help.PutStringInInput(By.Id("billing_email"), customer.emailAddress);

            help.TakeScreensot("entered_customer_details");
        }

        public void PlaceOrder()
        {
            MyHelpers help = new MyHelpers(driver);
            IWebElement? orderButton;

            orderButton = help.WaitForStaleElement(By.Id("place_order"), 3000, 500);

            TestContext.WriteLine("Place order");

            // Just awful design, but stale element exceptions are equally so.
            if (orderButton is not null)
            {
                try
                {
                    driver.FindElement(By.Id("place_order")).Click();
                }
                catch (StaleElementReferenceException)
                {
                    TestContext.WriteLine("Caught for the second time");
                    orderButton = help.WaitForStaleElement(By.Id("place_order"), 3000, 500);
                    driver.FindElement(By.Id("place_order")).Click();
                }
            }
            else
            {
                TestContext.WriteLine("Place order button not available." +
                    "Order not placed.");
                help.TakeScreensot("could_not_place_order");
            }
        }

        public string GetOrderNumber()
        {
            MyHelpers help = new MyHelpers(driver);
            // Wait for this element to appear.
            help.WaitForElement(By.CssSelector("li.order"), 2);
            string orderNumber = driver.FindElement(By.CssSelector("li.order"))
                .Text;

            // Cut stuff that isn't the order number off.
            // This is likely to be brittle.
            orderNumber = orderNumber.Substring(15);

            TestContext.WriteLine($"Order number = {orderNumber}");
            return orderNumber;
        }
    }
}
