using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPWebAutomation_MSTests
{
    public class PropertiesCollection
    {

        public static ExtentReports extent;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest test;
        public TestContext TestContext { get; set; }

    }
}
