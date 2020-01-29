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
    public class InsertStripDoc
    {
        String strtblname = "automation_stripdocument";
        String strTestType = "Regression";
        int intCurStripReportCount;

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["InsertStripDocumentEndPoint"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["InsertStripDocumentResource"], Method.POST);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body
            //request.AddXmlBody(new StripDocuments() { Data = testData[6], FileName = testData[9], StripID = testData[4], Title = testData[7], Notes = testData[5] });
            request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<StripDocuments>\n  <Data>"+ testData[6] +"</Data>\n  <FileName>"+ testData[9]+"</FileName>\n  <StripID>"+ testData[4] +"</StripID>\n  <Title>"+ testData[7] +"</Title>\n  <Notes>"+ testData[5]+"</Notes>\n</StripDocuments>", ParameterType.RequestBody);

            //Posting the Request
            IRestResponse response = client.Execute(request);

            return response;
        }
        
        public int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }

        //Function for validating invalid inputs -- Negative Scenarios
        public void ValidateData(String strTestCaseNo)
        {
            switch (strTestCaseNo)
            {
                case "Insert_TC002":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_InsertStripDoc_InvalidStripID");
                    break;
                case "Insert_TC003":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_InsertStripDoc_NULLTitle");
                    break;
                case "Insert_TC004":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_InsertStripDoc_TitleNotUnique");
                    break;
                case "Insert_TC005":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_InsertStripDoc_NullFileName");
                    break;
                case "Insert_TC006":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_InsertStripDoc_NullData");
                    break;
            }

            //Posting the Request
            IRestResponse response = CreateRestRequest(strTestCaseNo);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 400)
            {
                try
                {
                    if (strTestCaseNo == "Insert_TC002" || strTestCaseNo == "Insert_TC004")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");
                    } else if(strTestCaseNo == "Insert_TC003" || strTestCaseNo == "Insert_TC005" || strTestCaseNo == "Insert_TC006")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP200");
                    }
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Error code has passed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error code received is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "Document couldn't be inserted. Please check your input data");
                    PropertiesCollection.test.Log(Status.Pass, "Error Message received is: " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Error code has failed.");
                    PropertiesCollection.test.Log(Status.Fail, "Error Code is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Fail, "Error message received is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
            }
            else if (intRespCode == 200)
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Document got inserted for wrong input data.");
                PropertiesCollection.test.Log(Status.Fail, "Title updated is:  " + doc.GetElementsByTagName("Title")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Notes updated is:  " + doc.GetElementsByTagName("Notes")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "File name is:  " + doc.GetElementsByTagName("Filename")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Strip Report ID is:  " + doc.GetElementsByTagName("ID")[0].InnerText);
            }
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Please check your input");
                PropertiesCollection.test.Log(Status.Fail, "Error code is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
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
        
        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_InsertStripDoc_InsertDoc()
        {

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_InsertStripDoc_InsertDoc");
            Object[] ObjDBResponse = new object[7];

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, "Insert_TC001", strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            SqlDataReader reader = null;
            String strQuery = "select count(*) from dbo.tblstripreport where stripid = '" + testData[4] + "'";
            SqlCommand command = new SqlCommand(strQuery, myConnection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                intCurStripReportCount = reader.GetInt32(0);
            }
            reader.Close();

            IRestResponse response = CreateRestRequest("Insert_TC001");
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 200)
            //Success Response
            {
                strQuery = "select count(*) from dbo.tblstripreport where stripid = '" + testData[4] + "'";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                int newStripReportCount = 0;

                while (reader.Read())
                {
                    newStripReportCount = reader.GetInt32(0);
                }

                reader.Close();

                try
                {
                    Assert.AreNotEqual(intCurStripReportCount, newStripReportCount);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for count of Strip documents is passed");

                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for count of Strip documents is not passed.");
                }

                strQuery = "select * from dbo.tblstripreport where stripid = '" + testData[4] + "' and StripReportTitle = '" + testData[7] + "'";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ObjDBResponse[0] = reader[0].ToString(); //StripReportID
                    ObjDBResponse[1] = reader[1].ToString(); //StripID
                    ObjDBResponse[2] = reader[3].ToString(); //StripReportDated
                    ObjDBResponse[3] = reader[4].ToString(); //Title
                    ObjDBResponse[4] = reader[5].ToString(); //Notes
                    ObjDBResponse[5] = reader[6].ToString(); //File name
                    ObjDBResponse[6] = reader[9].ToString(); //Login name
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Notes")[0].InnerText, testData[5]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Document Notes is passed");
                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Document Notes is not passed");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Title")[0].InnerText, testData[7]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Document Title is passed");

                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Document Title is not passed");
                }
                try
                {
                    Assert.AreEqual(ObjDBResponse[6], "AFMIS");
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Login name is passed");

                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Login name is not passed. Expected is AFMIS. Actual is: " + ObjDBResponse[6]);
                }
                try
                {
                    String[] strDocType = (testData[9].ToString()).Split('.');
                    String strFileName = testData[7] + "-" + ObjDBResponse[0] + "." + strDocType[1];
                    Assert.AreEqual(doc.GetElementsByTagName("Filename")[0].InnerText, strFileName);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for File name is passed");

                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for File name is not passed");
                }
            }

            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Insert Strip document API not passed. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }

        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC02_InsertStripDoc_InvalidStripID()
        {
            String strTestCaseNo = "Insert_TC002";
            ValidateData(strTestCaseNo);            
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC03_InsertStripDoc_NULLTitle()
        {
            String strTestCaseNo = "Insert_TC003";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_InsertStripDoc_TitleNotUnique()
        {
            String strTestCaseNo = "Insert_TC004";
            ValidateData(strTestCaseNo);            
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_InsertStripDoc_NullFileName()
        {
            String strTestCaseNo = "Insert_TC005";
            ValidateData(strTestCaseNo);            
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_InsertStripDoc_NullData()
        {
            String strTestCaseNo = "Insert_TC006";
            ValidateData(strTestCaseNo);            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
