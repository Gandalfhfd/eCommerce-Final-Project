using FinalProject.Utils;
using OpenQA.Selenium;

namespace FinalProject.POMClasses
{
    internal class ShopPOM
    {
        private IWebDriver driver;
        public ShopPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool AddProductToCart(string productName)
        {
            string productSelectorString = $"Add “{productName}” to your cart";
            TestContext.WriteLine(productSelectorString);

            MyHelpers help = new MyHelpers(driver);
            try
            {
                driver.FindElement(By.CssSelector
                    ($"a[aria-label='{productSelectorString}']"))
                    .Click();
            }
            catch (Exception)
            {
                TestContext.WriteLine($"Could not add {productName} to cart");
                help.TakeScreensot($"Failed_to_add_{productName}_to_cart");
                return false;
            }

            // Wait for product to be added to basket before moving on.
            help.WaitForElement(By.CssSelector("a.added_to_cart"), 60);

            return true;
        }
    }
}
