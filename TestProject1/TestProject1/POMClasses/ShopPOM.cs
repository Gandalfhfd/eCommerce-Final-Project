using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class ShopPOM
    {
        private By btnViewCart = By.CssSelector("a.added_to_cart");
        private string _productSelectorString;
        private By btnAddToCart => By.CssSelector($"a[aria-label='{_productSelectorString}']");

        private IWebDriver driver;

        public ShopPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool AddProductToCart(string productName)
        {
            _productSelectorString = $"Add “{productName}” to your cart";
            TestContext.WriteLine(_productSelectorString);

            MyHelpers help = new MyHelpers(driver);
            try
            {
                driver.FindElement(btnAddToCart).Click();
            }
            catch (Exception)
            {
                TestContext.WriteLine($"Could not add {productName} to cart");
                help.TakeScreensot($"Failed_to_add_{productName}_to_cart");
                return false;
            }

            // Wait for product to be added to basket before moving on.
            help.WaitForElement(btnAddToCart, 6);

            return true;
        }
    }
}
