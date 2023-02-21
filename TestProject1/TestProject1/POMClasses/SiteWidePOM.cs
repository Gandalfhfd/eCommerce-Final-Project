﻿using OpenQA.Selenium;
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

        public SiteWidePOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateUsingNavLink(string linkText)
        {
            Console.WriteLine($"Click on \"{linkText}\" link");
            driver.FindElement(By.LinkText(linkText)).Click();
        }
    }
}
