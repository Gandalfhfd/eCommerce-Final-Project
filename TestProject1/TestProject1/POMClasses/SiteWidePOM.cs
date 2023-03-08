using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.POMClasses
{
    internal class SiteWidePOM
    {
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        private string _linkText;
        private By lnkLink => By.LinkText(_linkText);

        public SiteWidePOM(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public void NavigateUsingNavLink(string linkText)
        {
            _linkText = linkText;
            _specFlowOutputHelper.WriteLine($"Click on \"{linkText}\" link");
            _driver.FindElement(lnkLink).Click();
        }
    }
}
