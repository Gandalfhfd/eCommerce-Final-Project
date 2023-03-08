using FinalProject.Classes;
using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class CheckoutPOM
    {
        private readonly By _txtFirstName = By.Id("billing_first_name");
        private readonly By _txtLastName = By.Id("billing_last_name");
        private readonly By _txtAddress1 = By.Id("billing_address_1");
        private readonly By _txtTownCity = By.Id("billing_city");
        private readonly By _txtPostcode = By.Id("billing_postcode");
        private readonly By _txtPhoneNumber = By.Id("billing_phone");
        private readonly By _txtEmail = By.Id("billing_email");
        private readonly By _btnPlaceOrder = By.Id("place_order");
        private readonly By _lblOrderNumber = By.CssSelector("li.order");

        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public CheckoutPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public void EnterDetails()
        {
            // Instantiating grabs customer details from runsettings file.
            Customer customer = new Customer();

            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);

            _specFlowOutputHelper.WriteLine("Enter customer details");

            help.PutStringInInput(_txtFirstName, customer.FirstName);
            help.PutStringInInput(_txtLastName, customer.LastName);
            help.PutStringInInput(_txtAddress1, customer.StreetAddress);
            help.PutStringInInput(_txtTownCity, customer.TownCity);
            help.PutStringInInput(_txtPostcode, customer.Postcode);
            help.PutStringInInput(_txtPhoneNumber, customer.PhoneNumber);
            help.PutStringInInput(_txtEmail, customer.EmailAddress);

            help.TakeScreenshot("entered_customer_details");
        }

        public void PlaceOrder()
        {
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            IWebElement? orderButton;

            orderButton = help.WaitForStaleElement(_btnPlaceOrder, 3000, 500);

            _specFlowOutputHelper.WriteLine("Place order");

            // This element often goes stale twice, so the try catch handles that.
            if (orderButton is not null)
            {
                try
                {
                    _driver.FindElement(_btnPlaceOrder).Click();
                }
                catch (StaleElementReferenceException)
                {
                    _specFlowOutputHelper.WriteLine("Caught for the second time");
                    orderButton = help.WaitForStaleElement(_btnPlaceOrder, 3000, 500);
                    _driver.FindElement(_btnPlaceOrder).Click();
                }
            }
            else
            {
                _specFlowOutputHelper.WriteLine("Place order button not available." +
                    "Order not placed.");
                help.TakeScreenshot("could_not_place_order");
            }
        }

        public string GetOrderNumber()
        {
            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            // Wait for this element to appear.
            help.WaitForElement(_lblOrderNumber, 2);
            string orderNumber = _driver.FindElement(_lblOrderNumber).Text;

            // Extract just the order number.
            orderNumber = orderNumber.Substring(15);

            _specFlowOutputHelper.WriteLine($"Order number = {orderNumber}");
            return orderNumber;
        }
    }
}
