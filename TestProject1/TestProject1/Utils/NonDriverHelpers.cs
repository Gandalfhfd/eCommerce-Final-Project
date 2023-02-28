using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Utils
{
    internal class NonDriverHelpers
    {
        public string LoadParameterFromRunsettings(string parameterName)
        {
            string? output = TestContext.Parameters[parameterName];
            if (output is null)
            {
                TestContext.WriteLine($"Parameter <{parameterName}> not found");
                output = $"Parameter <{parameterName}> not found";
            }
            return output;
        }

        public string LoadEnvironmentVariable(string variableName)
        {
            string? output = Environment.GetEnvironmentVariable(variableName);
            if (output is null)
            {
                TestContext.WriteLine($"Environment variable <{variableName}> not found");
                output = $"Environment variable <{variableName}> not found";
            }
            return output;
        }

        public int FindMaxIterations(int timeToWaitInSeconds,
            int iterationLengthInMilliseconds)
        {
            // Calculate maximum number of iterations as a non-whole number.
            double nonIntegerMaxIter =
                1000 * timeToWaitInSeconds / iterationLengthInMilliseconds;

            // Round up and cast to int
            int maxIter = Convert.ToInt32(Math.Ceiling(nonIntegerMaxIter));
            return maxIter;
        }

        public string DateTimeNow()
        // Provide date and time in ISO 8601 format.
        {
            DateTime date = DateTime.Now;
            string output = date.ToString("yyyy-MM-ddTHHmmss");

            return output;
        }

        public decimal ConvertStringPercentToDecimal(string percentageString)
        {
            // Convert a string in the format xx% to 0.xx.
            // For example, 15% becomes 0.15.

            decimal output;

            // Remove the %.
            string barePercent = Truncate(percentageString, 1);

            // For example, convert 15 to 0.15 with the type decimal.
            output = Convert.ToDecimal(barePercent) * 0.01m;

            return output;
        }

        public string Truncate(string text, int numCharsToBeTruncated)
        {
            return text.Substring(0, text.Length - numCharsToBeTruncated);
        }
    }
}
