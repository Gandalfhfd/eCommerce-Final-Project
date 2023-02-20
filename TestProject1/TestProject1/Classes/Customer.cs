using FinalProject.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Classes
{
    internal class Customer
    {
        private IWebDriver driver;
        public string firstName { get; }
        public string lastName { get; }
        public string streetAddress { get; }
        public string townCity { get; }
        public string postcode { get; }
        public string phoneNumber { get; }
        public string emailAddress { get; }

        public string orderNumber { get; set; }

        public Customer(IWebDriver driver)
        {
            this.driver = driver;
            MyHelpers help = new MyHelpers(driver);

            this.firstName = help.LoadParameterFromRunsettings("firstName");
            this.lastName = help.LoadParameterFromRunsettings("lastName");
            this.streetAddress = help.LoadParameterFromRunsettings("streetAddress");
            this.townCity = help.LoadParameterFromRunsettings("townCity");
            this.postcode = help.LoadParameterFromRunsettings("postcode");
            this.phoneNumber = help.LoadParameterFromRunsettings("phoneNumber");
            this.emailAddress = help.LoadParameterFromRunsettings("emailAddress");

            this.orderNumber = "Order Number Not Set";
        }
    }
}
