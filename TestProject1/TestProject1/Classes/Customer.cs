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
        public string FirstName { get; }
        public string LastName { get; }
        public string StreetAddress { get; }
        public string TownCity { get; }
        public string Postcode { get; }
        public string PhoneNumber { get; }
        public string EmailAddress { get; }

        public string OrderNumber { get; set; }

        public Customer()
        {
            NonDriverHelpers nonDriverHelp = new NonDriverHelpers();

            FirstName = NonDriverHelpers.LoadParameterFromRunsettings("firstName");
            LastName = NonDriverHelpers.LoadParameterFromRunsettings("lastName");
            StreetAddress = NonDriverHelpers.LoadParameterFromRunsettings("streetAddress");
            TownCity = NonDriverHelpers.LoadParameterFromRunsettings("townCity");
            Postcode = NonDriverHelpers.LoadParameterFromRunsettings("postcode");
            PhoneNumber = NonDriverHelpers.LoadParameterFromRunsettings("phoneNumber");
            EmailAddress = NonDriverHelpers.LoadParameterFromRunsettings("emailAddress");
            
            OrderNumber = "Order Number Not Set";
        }
    }
}
