using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.POMClasses
{
    internal class SiteWidePOM
    {
        private IWebDriver driver;

        private string _linkText;
        private By lnkLink => By.LinkText(_linkText);

        public SiteWidePOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateUsingNavLink(string linkText)
        {
            _linkText = linkText;
            TestContext.WriteLine($"Click on \"{linkText}\" link");
            driver.FindElement(lnkLink).Click();
        }
    }
}
