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
        public string firstName { get; }
        public string lastName { get; }
        public string streetAddress { get; }
        public string townCity { get; }
        public string postcode { get; }
        public string phoneNumber { get; }
        public string emailAddress { get; }

        public string orderNumber { get; set; }

        public Customer()
        {
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();

            this.firstName = nonDriverHelp.LoadParameterFromRunsettings("firstName");
            this.lastName = nonDriverHelp.LoadParameterFromRunsettings("lastName");
            this.streetAddress = nonDriverHelp.LoadParameterFromRunsettings("streetAddress");
            this.townCity = nonDriverHelp.LoadParameterFromRunsettings("townCity");
            this.postcode = nonDriverHelp.LoadParameterFromRunsettings("postcode");
            this.phoneNumber = nonDriverHelp.LoadParameterFromRunsettings("phoneNumber");
            this.emailAddress = nonDriverHelp.LoadParameterFromRunsettings("emailAddress");

            this.orderNumber = "Order Number Not Set";
        }
    }
}
