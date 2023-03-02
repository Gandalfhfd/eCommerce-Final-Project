using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class MyAccountPOM
    {
        private By lblOrderNumber = By.CssSelector("tr > .woocommerce-orders-table__cell-order-number > a");
        private IWebDriver driver;
        public MyAccountPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string GetRecentOrderNumber()
        {
            string orderNumber = driver.FindElement(lblOrderNumber).Text;

            TestContext.WriteLine($"Order number from My account = {orderNumber}");
            // Return just the order number by removing a #.
            return orderNumber.Substring(1);
        }
    }
}
