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
    public class ReadStripTaskTemplate
    {
        String strtblname = "automation_striptasktemplate";
        String strTestType = "Regression";

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            PropertiesCollection.htmlReporter = new ExtentHtmlReporter(@"C:\Report\Report.html");
            //PropertiesCollection.htmlReporter.LoadConfig(@"C:\extent-configfile\extent-config.xml");
            string currentDir = Directory.GetCurrentDirectory();
            var configFile = currentDir.Replace("bin\\Debug", "extent-config.xml");

            PropertiesCollection.htmlReporter.LoadConfig(configFile);

            PropertiesCollection.extent = new AventStack.ExtentReports.ExtentReports();
            PropertiesCollection.extent.AttachReporter(PropertiesCollection.htmlReporter);
            PropertiesCollection.extent.AddSystemInfo("Automation Database", "8.1");
            PropertiesCollection.extent.AddSystemInfo("Application Under Test (AUT)", "FlightPro API");
        }
                
        [TestMethod]
        public void TC01_ReadStripTaskTemplate()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStripTaskTemplate");

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripTaskTemplateCount = doc.GetElementsByTagName("StripTaskTemplate").Count;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;
                        
            try
            {
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            /* Get row count from SQL Server */
            
            String TableName = "tblDefaultTaskText";

            var conn = new ConnectToSQLServer();
            int count = conn.Count(TableName, null, null);

            try
            {               
                Assert.AreEqual(count, StripTaskTemplateCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strip Task Templates displayed in response is " + StripTaskTemplateCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates displayed in the response is not as per the database");
            }
            
        }

        [TestMethod]
        public void TC02_ReadStripTaskTemplate_PaneID()
        {
            String strTestCaseNo = "Read_TC002";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_ReadStripTaskTemplate_PaneID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            
            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
            
            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);

            //Executing the Request and getting the response
            var response = client.Execute(request);
            
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripTaskTemplateCount = doc.GetElementsByTagName("StripTaskTemplate").Count;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            /* Get row count from SQL Server */

            String TableName = "tblDefaultTaskTextPane";
            String TableColumn = "PaneID";
            String WhereCondition = strTDPaneID;

            var conn = new ConnectToSQLServer();
            int count = conn.Count(TableName, TableColumn, WhereCondition);

            try
            {
                Assert.AreEqual(count, StripTaskTemplateCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strip Task Templates displayed in response is " + StripTaskTemplateCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates displayed in the response is not as per the database");
            }

        }

        [TestMethod]
        public void TC03_ReadStripTaskTemplate_AssetTypeID()
        {
            String strTestCaseNo = "Read_TC003";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_ReadStripTaskTemplate_AssetTypeID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            string strTDAssetTypeID = testdata_details[4];
               
            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
            
            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);
            request.AddParameter("AssetTypeID", strTDAssetTypeID);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripTaskTemplateCount = doc.GetElementsByTagName("StripTaskTemplate").Count;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            /* Get row count from SQL Server */

            var conn = new ConnectToSQLServer();
            int count = conn.StripTaskTemplateCountforAssetTypeID(strTDAssetTypeID);

            try
            {
                Assert.AreEqual(count, StripTaskTemplateCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strip Task Templates displayed in response is " + StripTaskTemplateCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates displayed in the response is not as per the database");
            }

        }

        [TestMethod]
        public void TC04_ReadStripTaskTemplate_InvalidPaneID()
        {
            String strTestCaseNo = "Read_TC004";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_ReadStripTaskTemplate_InvalidPaneID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            string strTDResult = testdata_details[5];

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripTaskTemplateCount = doc.GetElementsByTagName("StripTaskTemplate").Count;
            

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(0, StripTaskTemplateCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strip Task Templates displayed in response is " + StripTaskTemplateCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates displayed is not 0");
            }

            try
            {
                Assert.AreEqual(response.Content, strTDResult);
                PropertiesCollection.test.Log(Status.Pass, "Response is validated and passed");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Response is validated and failed");
            }
        }

        [TestMethod]
        public void TC05_ReadStripTaskTemplate_InvalidAssetTypeID()
        {
            String strTestCaseNo = "Read_TC005";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_ReadStripTaskTemplate_InvalidAssetTypeID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            string strTDAssetTypeID = testdata_details[4];
            string strTDResult = testdata_details[5];

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);
            request.AddParameter("AssetTypeID", strTDAssetTypeID);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripTaskTemplateCount = doc.GetElementsByTagName("StripTaskTemplate").Count;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;
            Console.WriteLine(intRespCode);

            try
            {
                Assert.AreEqual(intRespCode, 200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(0, StripTaskTemplateCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strip Task Templates displayed in response is " + StripTaskTemplateCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates is not 0");
            }

            try
            {
                Assert.AreEqual(response.Content, strTDResult);
                PropertiesCollection.test.Log(Status.Pass, "Response is validated and passed");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Response is validated and failed");
            }
        }

        [TestMethod]
        public void TC06_ReadStripTaskTemplate_InvalidFormat_PaneID()
        {
            String strTestCaseNo = "Read_TC006";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_ReadStripTaskTemplate_InvalidFormat_PaneID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            string strTDCode = testdata_details[6];

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 400);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(Code, strTDCode);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }
        }

            [TestMethod]
        public void TC07_ReadStripTaskTemplate_InvalidFormat_AssetTypeID()
        {
            String strTestCaseNo = "Read_TC007";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC07_ReadStripTaskTemplate_InvalidFormat_AssetTypeID");

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();

            var testdata_details = connection.Select(strtblname, strTestCaseNo, strTestType);

            string strTDPaneID = testdata_details[3];
            string strTDAssetTypeID = testdata_details[4];
            string strTDCode = testdata_details[6];

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("StripTaskTemplates/", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding parameter to the request
            request.AddParameter("PaneID", strTDPaneID);
            request.AddParameter("AssetTypeID", strTDAssetTypeID);

            //Executing the Request and getting the response
            var response = client.Execute(request);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 400);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(Code, strTDCode);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

        }

    
        public void TestCleanup()
        {
            var status = TestContext.CurrentTestOutcome;
            Status status1;

            //   LogSatus logstatus;

            Console.WriteLine("TestContext:" + TestContext.CurrentTestOutcome);

            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                status1 = Status.Fail;
                PropertiesCollection.test.Log(Status.Fail, "Test Failed and aborted");
            }

            System.Threading.Thread.Sleep(4000);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
