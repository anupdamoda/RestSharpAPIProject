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

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class DeleteStripDoc
    {
        String strtblname = "automation_stripdocument";
        String strTestType = "Regression";
        

        public IRestResponse DeleteRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["UpdateStripDocumentEndPoint"]);
            var request = new RestRequest(testData[3], Method.DELETE);

            //Adding headers to the DELETE request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

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
            PropertiesCollection.htmlReporter.LoadConfig(@"C:\extent-configfile\extent-config.xml");
            PropertiesCollection.extent = new AventStack.ExtentReports.ExtentReports();
            PropertiesCollection.extent.AttachReporter(PropertiesCollection.htmlReporter);
            PropertiesCollection.extent.AddSystemInfo("Automation Database", "8.1");
            PropertiesCollection.extent.AddSystemInfo("Application Under Test (AUT)", "FlightPro API");
        }



        [TestMethod]
        public void TC01_DeleteStripDoc_DeleteDoc()
        {
            String strTestCaseNo = "Delete_TC001";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_DeleteStripDoc_DeleteDoc");
            Object[] ObjDBResponse = new object[7];

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            IRestResponse response = DeleteRestRequest(strTestCaseNo);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            SqlDataReader reader = null;
            String query;
            SqlCommand command;

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

            if (intRespCode == 200)
            {
                Object[] dbResponse = new object[7];

                query = "select count(*) from dbo.tblstripreport where StripReportID = '" + testData[3] + "'";
                command = new SqlCommand(query, myConnection);
                reader = command.ExecuteReader();

                try
                {
                    Assert.AreEqual(query,0);
                    PropertiesCollection.test.Log(Status.Pass, "Strip Document has been deleted from the Database");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Strip Document has not been deleted from the Database");
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP004");
                    PropertiesCollection.test.Log(Status.Pass, "Validation of the Code has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation of the Code has failed");
                }
            }
        }

        [TestMethod]
        public void TC02_DeleteStripDoc_DeleteDoc_MultipleDocs()
        {
            String strTestCaseNo = "Delete_TC002";

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_DeleteStripDoc_DeleteDoc_MultipleDocs");
            Object[] ObjDBResponse = new object[7];

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);
            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            IRestResponse response = DeleteRestRequest(strTestCaseNo);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            String query;
            SqlCommand command;
            SqlDataReader reader = null;

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

            if (intRespCode == 200)
            {
                Object[] dbResponse = new object[7];

                query = "select count(*) from dbo.tblstripreport where StripReportID = '" + testData[3] + "'";
                command = new SqlCommand(query, myConnection);
                reader = command.ExecuteReader();

                try
                {
                    Assert.AreEqual(query, 0);
                    PropertiesCollection.test.Log(Status.Pass, "Strip Document has been deleted from the Database");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Strip Document has not been deleted from the Database");
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP004");
                    PropertiesCollection.test.Log(Status.Pass, "Validation of the Code has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation of the Code has failed");
                }
            }
        }

        [TestMethod]
        public void TC03_DeleteStripDoc_InvalidURL()
        {
            String strTestCaseNo = "Delete_TC003";
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_DeleteStripDoc_InvalidURL");
            Object[] ObjDBResponse = new object[7];

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, strTestCaseNo, strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            IRestResponse response = DeleteRestRequest(strTestCaseNo);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            String query;
            SqlCommand command;
            SqlDataReader reader = null;

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);


            if (intRespCode == 400)
            {
                Object[] dbResponse = new object[7];

                query = "select count(*) from dbo.tblstripreport where StripReportID = '" + testData[3] + "'";
                command = new SqlCommand(query, myConnection);
                reader = command.ExecuteReader();

                try
                {
                    Assert.AreNotEqual(query, 0);
                    PropertiesCollection.test.Log(Status.Pass, "Strip Document has not been deleted from the Database");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Strip Document has been deleted from the Database");
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP101");
                    PropertiesCollection.test.Log(Status.Pass, "Validation of the Code has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation of the Code has failed");
                }

            }
        }

        [TestMethod]


        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
