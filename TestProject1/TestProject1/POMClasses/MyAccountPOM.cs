using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class MyAccountPOM
    {
        private By lblOrderNumber = By.CssSelector("tr > .woocommerce-orders-table__cell-order-number > a");
        private IWebDriver driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public MyAccountPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            this.driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public string GetRecentOrderNumber()
        {
            string orderNumber = driver.FindElement(lblOrderNumber).Text;

            _specFlowOutputHelper.WriteLine($"Order number from My account = {orderNumber}");
            // Return just the order number by removing a #.
            return orderNumber.Substring(1);
        }
    }
}
