using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using FPWebAutomation_MSTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Data.SqlClient;
using System.Net;
using UnitTestProject3.Database;
using System.Configuration;
using System.Xml;
using System.IO;

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class ReadStripDoc
    {
        String strtblname = "automation_stripdocument";
        String strTestType = "Regression";
        

        public IRestResponse ReadRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["UpdateStripDocumentEndPoint"]);
            //var client = new RestClient("http://oc-svr-at1/Fltpro_Automation_main/API/v1/StripDocuments/");
            var request = new RestRequest(testData[3], Method.GET);


            //Adding headers to the DELETE request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //PropertiesCollection.test.Log(Status.Fail, "URL" + requestu);

            //Deleting the Request
            IRestResponse response = client.Execute(request);

            return response;
        }


        public int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }

        

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            PropertiesCollection.htmlReporter = new ExtentHtmlReporter(@"C:\Report\Report.html");
            string currentDir = Directory.GetCurrentDirectory();
            var configFile = currentDir.Replace("bin\\Debug", "extent-config.xml");
            PropertiesCollection.htmlReporter.LoadConfig(configFile);
            PropertiesCollection.extent = new AventStack.ExtentReports.ExtentReports();
            PropertiesCollection.extent.AttachReporter(PropertiesCollection.htmlReporter);
            PropertiesCollection.extent.AddSystemInfo("Automation Database", "8.1");
            PropertiesCollection.extent.AddSystemInfo("Application Under Test (AUT)", "FlightPro API");
        }



        [TestMethod]
        public void TC01_ReadStripDoc_ReadDoc()
        {
            String strTestCaseNo = "Read_TC003";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStripDoc_ReadDoc");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);

            IRestResponse response = ReadRestRequest(strTestCaseNo);
            PropertiesCollection.test.Log(Status.Pass, "Response" + response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            try
            {
                Console.WriteLine("intResponse: " + intRespCode);
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

        }

        [TestMethod]
        public void TC01_ReadStripDoc_ReadDoc_InvalidFormatOfStripReportID()
        {
            String strTestCaseNo = "Read_TC002";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStripDoc_ReadDoc");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);

            IRestResponse response = ReadRestRequest(strTestCaseNo);
            PropertiesCollection.test.Log(Status.Pass, "Response" + response.Content);

            try
            {
                // Fetching the response content to check that the StripReportID has invalid format
                var responseContent = response.Content;
                Assert.IsTrue(responseContent.Contains("FP105"));
                PropertiesCollection.test.Log(Status.Pass, "Status Response is not 400 due to invalid format of StripReportID");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 400");
            }

        }

        [TestMethod]
        public void TC01_ReadStripDoc_ReadDoc_InvalidStripReportID()
        {
            String strTestCaseNo = "Read_TC004";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStripDoc_ReadDoc");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);

            IRestResponse response = ReadRestRequest(strTestCaseNo);
            PropertiesCollection.test.Log(Status.Pass, "Response" + response.Content);

            try
            {
                // Fetching the response content to check that the StripReportID has invalid format
                var responseContent = response.Content;
                Assert.IsTrue(responseContent.Contains("FP101"));
                PropertiesCollection.test.Log(Status.Pass, "Status Response is not 400 due to invalid format of StripReportID");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 400");
            }

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
