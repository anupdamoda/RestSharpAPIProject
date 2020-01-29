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
    public class UpdateStripDoc
    {
        String strTblName = "automation_stripdocument";
        String strTestType = "Regression";

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strTblName, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["UpdateStripDocumentEndPoint"]);
            var request = new RestRequest(testData[3], Method.PUT);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body
            //request.AddXmlBody(new StripDocuments() { Data = testData[6], FileName = testData[9], StripID = testData[4], Title = testData[7], Notes = testData[5] });
            request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<StripDocuments>\n  <Data>"+testData[6]+"</Data>   \n  <FileName>"+ testData[9] +"</FileName>\n  <StripID>"+ testData[4] +"</StripID>\n  <Title>"+ testData[7] +"</Title>\n  <Notes>"+ testData[5] +"</Notes>\n</StripDocuments>\n", ParameterType.RequestBody);

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

        //Function for validating invalid inputs -- Negative Scenarios
        public void ValidateData(String strTestCaseNo)
        {
            switch(strTestCaseNo)
            {
                case "Update_TC003":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_UpdateStripDoc_NullFileName");
                    break;
                case "Update_TC004":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_UpdateStripDoc_InvalidStripReportID");
                    break;
                case "Update_TC005":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_UpdateStripDoc_DocumentSavedPriortoUpdate");
                    break;
                case "Update_TC006":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_UpdateStripDoc_InvalidFileExtension");
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
                    if (strTestCaseNo == "Update_TC003" || strTestCaseNo == "Update_TC006")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP105");
                    }
                    else if (strTestCaseNo == "Update_TC004" || strTestCaseNo == "Update_TC005")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");
                    }
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Error code has passed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error code is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "Document couldn't be updated. Please check your input data");
                    PropertiesCollection.test.Log(Status.Pass, "Error Message is: " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Error code has failed.");
                    PropertiesCollection.test.Log(Status.Fail, "Error Code is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
            }
            else if(intRespCode == 200)
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Document got updated for wrong input data.");
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

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_UpdateStripDoc_UpdateTitleAndNotes()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_UpdateStripDoc_UpdateTitleAndNotes");

            //Fetching data from My SQL Database
            var doc = new XmlDocument();
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strTblName, "Update_TC001", strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            SqlDataReader reader = null;
            String query; 
            SqlCommand command; 

            //Posting the Request
            IRestResponse response = CreateRestRequest("Update_TC001");
            doc.LoadXml(response.Content);
            
            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 200)
            {
                Object[] dbResponse = new object[7];

                query = "select * from dbo.tblstripreport where stripid = '" + testData[4] + "' and StripReportID = '" + testData[3] + "'";
                command = new SqlCommand(query, myConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dbResponse[0] = reader[0].ToString(); //StripReportID
                    dbResponse[1] = reader[1].ToString(); //StripID
                    dbResponse[2] = reader[3].ToString(); //StripReportDated
                    dbResponse[3] = reader[4].ToString(); //Title
                    dbResponse[4] = reader[5].ToString(); //Notes
                    dbResponse[5] = reader[6].ToString(); //File name
                    dbResponse[6] = reader[9].ToString(); //Login name
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Title")[0].InnerText, dbResponse[3]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Title name has passed.");

                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Title name has failed. Expected is: " + dbResponse[3] + ". Actual is: " + doc.GetElementsByTagName("Title")[0].InnerText);
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Notes")[0].InnerText, dbResponse[4]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for notes has passed");
                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for notes has not passed.");
                }

                try
                {
                    Assert.AreEqual(dbResponse[5], doc.GetElementsByTagName("Filename")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for File name has passed.");
                }
                catch 
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for File name has not passed. Expected is: ");
                }
            }
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Update Strip document API not passed. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC02_UpdateStripDoc_UpdateContentAndNotes()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_UpdateStripDoc_UpdateContentAndNotes");

            //Fetching data from My SQL Database
            String strNotes = "";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strTblName, "Update_TC002", strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            //Posting the Request
            IRestResponse response = CreateRestRequest("Update_TC002");
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 200)
            {
                String query = "select * from dbo.tblstripreport where stripid = '" + testData[4] + "' and StripReportID = '" + testData[3] + "'";
                SqlCommand command = new SqlCommand(query, myConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    strNotes = reader["StripReportNotes"].ToString();
                }                    
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Notes")[0].InnerText, strNotes);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for notes has passed");
                    PropertiesCollection.test.Log(Status.Pass, "Document update has been successful.");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for notes has not passed. Expected notes is: " + strNotes + "Actual notes from API Response is: " + doc.GetElementsByTagName("Notes")[0].InnerText);
                }
            }
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Update Strip document API not passed. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC03_UpdateStripDoc_NullFileName()
        {

            String strTestCaseNo = "Update_TC003";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_UpdateStripDoc_InvalidStripReportID()
        {
            String strTestCaseNo = "Update_TC004";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_UpdateStripDoc_DocumentSavedPriortoUpdate()
        {
            String strTestCaseNo = "Update_TC005";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_UpdateStripDoc_InvalidFileExtension()
        {
            String strTestCaseNo = "Update_TC006";
            ValidateData(strTestCaseNo);
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
