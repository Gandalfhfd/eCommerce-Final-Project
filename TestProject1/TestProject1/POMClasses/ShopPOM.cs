using FinalProject.Utils;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class ShopPOM
    {
        private readonly By _btnViewCart = By.CssSelector("a.added_to_cart");
        private string _productSelectorString;
        private By _btnAddToCart => By.CssSelector($"a[aria-label='{_productSelectorString}']");

        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        public ShopPOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public bool AddProductToCart(string productName)
        {
            // _productSelectorString is used in the locator to find the add to
            // cart button for a particular item.
            _productSelectorString = $"Add “{productName}” to your cart";
            _specFlowOutputHelper.WriteLine(_productSelectorString);

            MyHelpers help = new MyHelpers(_driver, _specFlowOutputHelper);
            try
            {
                _driver.FindElement(_btnAddToCart).Click();
            }
            catch (Exception)
            {
                _specFlowOutputHelper.WriteLine($"Could not add {productName} to cart");
                help.TakeScreenshot($"Failed_to_add_{productName}_to_cart");
                return false;
            }

            // Wait for product to be added to basket before moving on.
            help.WaitForElement(_btnViewCart, 6);

            return true;
        }
    }
}
