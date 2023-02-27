using FinalProject.Classes;
using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class CheckoutPOM
    {
        private By txtFirstName = By.Id("billing_first_name");
        private By txtLastName = By.Id("billing_last_name");
        private By txtAddress1 = By.Id("billing_address_1");
        private By txtTownCity = By.Id("billing_city");
        private By txtPostcode = By.Id("billing_postcode");
        private By txtPhoneNumber = By.Id("billing_phone");
        private By txtEmail = By.Id("billing_email");
        private By btnPlaceOrder = By.Id("place_order");
        private By lblOrderNumber = By.CssSelector("li.order");

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

            help.PutStringInInput(txtFirstName, customer.firstName);
            help.PutStringInInput(txtLastName, customer.lastName);
            help.PutStringInInput(txtAddress1, customer.streetAddress);
            help.PutStringInInput(txtTownCity, customer.townCity);
            help.PutStringInInput(txtPostcode, customer.postcode);
            help.PutStringInInput(txtPhoneNumber, customer.phoneNumber);
            help.PutStringInInput(txtEmail, customer.emailAddress);

            help.TakeScreensot("entered_customer_details");
        }

        public void PlaceOrder()
        {
            MyHelpers help = new MyHelpers(driver);
            IWebElement? orderButton;

            orderButton = help.WaitForStaleElement(btnPlaceOrder, 3000, 500);

            TestContext.WriteLine("Place order");

            // Just awful design, but stale element exceptions are equally so.
            if (orderButton is not null)
            {
                try
                {
                    driver.FindElement(btnPlaceOrder).Click();
                }
                catch (StaleElementReferenceException)
                {
                    TestContext.WriteLine("Caught for the second time");
                    orderButton = help.WaitForStaleElement(btnPlaceOrder, 3000, 500);
                    driver.FindElement(btnPlaceOrder).Click();
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
            help.WaitForElement(lblOrderNumber, 2);
            string orderNumber = driver.FindElement(lblOrderNumber).Text;

            // Cut stuff that isn't the order number off.
            // This is likely to be brittle.
            orderNumber = orderNumber.Substring(15);

            TestContext.WriteLine($"Order number = {orderNumber}");
            return orderNumber;
        }
    }
}
