using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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

        public void WaitForElement(By locator, int timeToWaitInSeconds)
        {
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(timeToWaitInSeconds));
            wait2.Until(drv => drv.FindElement(locator).Enabled);
        }

        public void WaitForScroll(int timeToWaitInSeconds,
            int timeBetweenChecksInMilliseconds = 500)
        {
            int scrollPosition1;
            int scrollPosition2;

            int maxIter = FindMaxIterations(timeToWaitInSeconds,
                timeBetweenChecksInMilliseconds);

            for (int i = 0; i < maxIter; i++)
            {
                scrollPosition1 = GetCurrentScrollPosition();
                Thread.Sleep(timeBetweenChecksInMilliseconds);
                scrollPosition2 = GetCurrentScrollPosition();

                if (scrollPosition1 != scrollPosition2)
                {
                    // Scroll has started/occurred, so end the function.
                    break;
                }
            }
        }

        private static int FindMaxIterations(int timeToWaitInSeconds,
            int iterationLengthInMilliseconds)
        {
            // Calculate maximum number of iterations as a non-whole number.
            double nonIntegerMaxIter = 
                1000 * timeToWaitInSeconds / iterationLengthInMilliseconds;

            // Round up and cast to int
            int maxIter = Convert.ToInt32(Math.Ceiling(nonIntegerMaxIter));
            return maxIter;
        }

        public int GetCurrentScrollPosition()
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            object scrollPosition = executor
                .ExecuteScript("return window.pageYOffset");

            // Convert scrollPosition to int.
            int scrollPositionInt = (int)Convert.ToInt64(scrollPosition);
            return scrollPositionInt;
        }

        public bool IsElementPresent(By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
