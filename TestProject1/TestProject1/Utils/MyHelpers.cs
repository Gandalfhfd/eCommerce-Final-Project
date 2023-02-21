using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

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

        public void PutStringInInput(IWebElement textField, string text)
        // Overload
        {
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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeToWaitInSeconds));
            wait.Until(drv => drv.FindElement(locator));
        }

        // Have some way of waiting for several poll times.
        public IWebElement? WaitForStaleElement(By locator,
            int totalTimeToWaitInMilliseconds,
            int timeElementShouldBeStableForInMilliseconds = 500,
            int timeToWaitBeforeRetryingInMilliseconds = 50)
        {
            IWebElement element;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            bool elementIsThere;

            // If we've waited longer than we want to, end the loop.
            while (stopWatch.ElapsedMilliseconds < totalTimeToWaitInMilliseconds)
            {
                try
                {
                    // Refresh the element.
                    element = driver.FindElement(locator);

                    // Check if element has gone stale.
                    // Exception will be thrown if it has, which will restart
                    // the loop.
                    elementIsThere = element.Enabled;

                    // If the element isn't stale, wait before
                    // checking it again.
                    // Otherwise, the element may be unstable and go stale
                    // before we pass it back.
                    Thread.Sleep(timeElementShouldBeStableForInMilliseconds);

                    // Check if element has gone stale.
                    elementIsThere = element.Enabled;

                    // Getting this far means the element exists and is stable,
                    // so return it.
                    Console.WriteLine("Stopwatch finished at: "
                        + stopWatch.ElapsedMilliseconds.ToString() + "ms");
                    stopWatch.Stop();

                    return driver.FindElement(locator); ;
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine("Element not stable - retry wait");
                    Thread.Sleep(timeToWaitBeforeRetryingInMilliseconds);
                }
            }

            // In case it hadn't already stopped.
            stopWatch.Stop();

            Console.WriteLine("WaitForStaleElement timed out");
            // Make sure what this is being passed back to can handle nulls
            return null;
        }

        public void WaitForScroll(int timeToWaitInSeconds,
            int pollingTimeInMilliseconds = 500)
        {
            int scrollPosition1;
            int scrollPosition2;

            int maxIter = FindMaxIterations(timeToWaitInSeconds,
                pollingTimeInMilliseconds);

            for (int i = 0; i < maxIter; i++)
            {
                scrollPosition1 = GetCurrentScrollPosition();
                Thread.Sleep(pollingTimeInMilliseconds);
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

        public void TakeScreensot(string screenshotName)
        {
            string screenshotDirectory = LoadParameterFromRunsettings("screenshotPath");

            string screenshotPath = @screenshotDirectory + screenshotName +
                DateTimeNow() + ".png";

            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();

            // Take screenshot. File name needs to be further configured.
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            TestContext.AddTestAttachment(screenshotPath, screenshotName);
        }

        public string DateTimeNow()
        // Provide date and time in ISO 8601 format.
        {
            DateTime date = DateTime.Now;
            string output = date.ToString("yyyy-MM-ddTHHmmss");

            return output;
        }
    }
}
