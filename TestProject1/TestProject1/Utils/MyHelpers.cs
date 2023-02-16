using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Utils
{
    internal class MyHelpers
    {
        private IWebDriver driver;
        public MyHelpers(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void PutStringInInput(By locator, string text)
        {
            IWebElement textField = driver.FindElement(locator);
            textField.Click();
            textField.Clear();
            textField.SendKeys(text);
        }

        public string LoadParameterFromRunsettings(string parameterName)
        {
            string? output = TestContext.Parameters[parameterName];
            if (output is null)
            {
                Console.WriteLine($"Parameter <{parameterName}> not found");
                output = $"Parameter <{parameterName}> not found";
            }
            return output;
        }
    }
}
