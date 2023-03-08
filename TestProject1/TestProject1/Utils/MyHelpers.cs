using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace FinalProject.Utils
{
    internal class MyHelpers
    {
        private readonly IWebDriver _driver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        public MyHelpers(IWebDriver driver, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _driver = driver;
            _specFlowOutputHelper = specFlowOutputHelper;
        }
        public void PutStringInInput(By locator, string text)
        {
            IWebElement textField = _driver.FindElement(locator);
            textField.Click();
            textField.Clear();
            textField.SendKeys(text);
        }

        public static void PutStringInInput(IWebElement textField, string text)
        // Overload
        {
            textField.Click();
            textField.Clear();
            textField.SendKeys(text);
        }

        public void WaitForElement(By locator, int timeToWaitInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeToWaitInSeconds));
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
                    element = _driver.FindElement(locator);

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
                    _specFlowOutputHelper.WriteLine("Stopwatch finished at: "
                        + stopWatch.ElapsedMilliseconds.ToString() + "ms");
                    stopWatch.Stop();

                    return _driver.FindElement(locator); ;
                }
                catch (StaleElementReferenceException)
                {
                    _specFlowOutputHelper.WriteLine("Element not stable - retry wait");
                    Thread.Sleep(timeToWaitBeforeRetryingInMilliseconds);
                }
            }

            // In case it hadn't already stopped.
            stopWatch.Stop();

            _specFlowOutputHelper.WriteLine("WaitForStaleElement timed out");
            TakeScreenshot("Element_Still_Stale");

            // What this is being passed back should be able to handle nulls.
            return null;
        }

        public void WaitForScroll(int timeToWaitInSeconds,
            int pollingTimeInMilliseconds = 500)
        {
            // Wait for the page to start scrolling.
            int scrollPosition1;
            int scrollPosition2;

            int maxIter = NonDriverHelpers.FindMaxIterations(timeToWaitInSeconds,
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

        public int GetCurrentScrollPosition()
        {
            // Find the scroll position.
            IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
            object scrollPosition = executor
                .ExecuteScript("return window.pageYOffset");

            // Convert scrollPosition to int.
            int scrollPositionInt = (int)Convert.ToInt64(scrollPosition);
            return scrollPositionInt;
        }

        public bool IsElementPresent(By locator, string elementName)
        {
            try
            {
                _driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                TakeScreenshot($"{elementName}_not_found");
                return false;
            }
        }

        public void TakeScreenshot(string screenshotName)
        {
            string screenshotDirectory = NonDriverHelpers.LoadEnvironmentVariable("screenshotPath");

            string screenshotPath = @screenshotDirectory + screenshotName +
                NonDriverHelpers.DateTimeNow() + ".png";

            ITakesScreenshot? ssdriver = _driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();

            // Take screenshot.
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            // Save to specflow livingdoc
            _specFlowOutputHelper.AddAttachment(screenshotPath);
            TestContext.AddTestAttachment(screenshotPath, screenshotName);
        }
    }
}
