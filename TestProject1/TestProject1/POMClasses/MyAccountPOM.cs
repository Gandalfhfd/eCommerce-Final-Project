using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class MyAccountPOM
    {
        private readonly By _lblOrderNumber = By.CssSelector("tr > .woocommerce-orders-table__cell-order-number > a");
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public MyAccountPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public string GetRecentOrderNumber()
        {
            string orderNumber = _driver.FindElement(_lblOrderNumber).Text;

            _specFlowOutputHelper.WriteLine($"Order number from My account = {orderNumber}");
            // Return just the order number by removing a #.
            return orderNumber.Substring(1);
        }
    }
}
