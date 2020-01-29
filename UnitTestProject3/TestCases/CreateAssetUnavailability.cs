/*Script name   : Create Strip - Asset Unavailability API Automation
  API Version   : 8.02.100
  Author        : Vandana Kalluru
  Dated         : 07th Aug 2019  */
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
using System.Collections.Generic;

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class CreateAssetUnavailability
    {
        String strtblname = "automation_createunavailability";
        String strTestType = "Regression";
        int intCurStripCount;
        SqlDataReader reader = null;
        SqlCommand command = null;
        String strQuery, strUnavailabilityDesc,strAssetTypeID, strAssetTail, strPlannedStartTime, strPlannedEndTime;
        DateTime plannedStartTime, plannedEndTime;

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            //Code to get current date & time and replace minutes with 05 starts
            if(TestCaseNo == "TC006_Asset")
            {
                plannedEndTime = DateTime.Now.ToUniversalTime();
                plannedStartTime = plannedEndTime.AddHours(2);
            }else
            {
                plannedStartTime = DateTime.Now.ToUniversalTime();
                plannedEndTime = plannedStartTime.AddHours(2);
            }
                       
            strPlannedStartTime = plannedStartTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ");   
            strPlannedEndTime = plannedEndTime.ToString("yyyy-MM-ddThh:mm:ss.fffZ");

            strPlannedStartTime = strPlannedStartTime.Remove(14, 2);
            strPlannedStartTime = strPlannedStartTime.Insert(14, "05");

            strPlannedEndTime = strPlannedEndTime.Remove(14, 2);
            strPlannedEndTime = strPlannedEndTime.Insert(14, "05");
            //Code to get current date & time and replace minutes with 05 ends

            var client = new RestClient(ConfigurationManager.AppSettings["InsertStripEndPoint"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["InsertStripResource"], Method.POST);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body
            if(TestCaseNo == "TC001_Asset" || TestCaseNo == "TC006_Asset" || TestCaseNo == "TC005_Asset" )
            {
                request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<Strip>\n  <AssetID>" + testData[4] + "</AssetID>\n  <SubTypeID>" + testData[5] + "</SubTypeID>\n  <Details>" + testData[6] + "</Details>\n  <Details2>" + testData[7] + "</Details2>\n  <PlannedStartTime>" + strPlannedStartTime + "</PlannedStartTime>\n  <PlannedEndTime>" + strPlannedEndTime + "</PlannedEndTime>\n  <Type>" + testData[8] + "</Type>\n</Strip>", ParameterType.RequestBody);
            }else if(TestCaseNo == "TC002_Asset")
            {
                request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<Strip>\n  <SubTypeID>" + testData[5] + "</SubTypeID>\n  <Details>" + testData[6] + "</Details>\n  <Details2>" + testData[7] + "</Details2>\n  <PlannedStartTime>" + strPlannedStartTime + "</PlannedStartTime>\n  <PlannedEndTime>" + strPlannedEndTime + "</PlannedEndTime>\n  <Type>" + testData[8] + "</Type>\n</Strip>", ParameterType.RequestBody);
            }
            else if (TestCaseNo == "TC003_Asset")
            {
                request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<Strip>\n  <AssetID>" + testData[4] + "</AssetID>\n  <Details>" + testData[6] + "</Details>\n  <Details2>" + testData[7] + "</Details2>\n  <PlannedStartTime>" + strPlannedStartTime + "</PlannedStartTime>\n  <PlannedEndTime>" + strPlannedEndTime + "</PlannedEndTime>\n  <Type>" + testData[8] + "</Type>\n</Strip>", ParameterType.RequestBody);
            }else if (TestCaseNo == "TC004_Asset")
            {
                request.AddParameter("undefined", "<?xml version=\"1.0\"?>\n<Strip>\n  <AssetID>" + testData[4] + "</AssetID>\n  <SubTypeID>" + testData[5] + "</SubTypeID>\n  <Details>" + testData[6] + "</Details>\n  <Details2>" + testData[7] + "</Details2>\n  <PlannedStartTime>" + strPlannedStartTime + "</PlannedStartTime>\n  <PlannedEndTime>" + strPlannedEndTime + "</PlannedEndTime>\n</Strip>", ParameterType.RequestBody);
            }
            
            //Triggering the Request
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
                case "TC002_Asset":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_CreateAssetUnavailability_NullAssetID");
                    break;
                case "TC003_Asset":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_CreateAssetUnavailability_NULLSubTypeID");
                    break;
                case "TC004_Asset":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_CreateAssetUnavailability_NULLStripType");
                    break;
                case "TC005_Asset":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_CreateAssetUnavailability_InvalidAssetID");
                    break;
                case "TC006_Asset":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_CreateAssetUnavailability_PlannedStartGreaterThanPlannedEndTime");
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
                    if (strTestCaseNo == "TC004_Asset" || strTestCaseNo == "TC006_Asset")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");
                    }
                    else if (strTestCaseNo == "TC002_Asset" || strTestCaseNo == "TC003_Asset")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP200");
                    }
                    else if (strTestCaseNo == "TC005_Asset")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP105");
                    }
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Error code has passed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error code received is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "Asset Unavailability couldn't be created. Please check your input data");
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
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Asset unavailability has been created with wrong input data.");
                PropertiesCollection.test.Log(Status.Fail, "Strip type created is: " + doc.GetElementsByTagName("Type")[2].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned Start time of the strip is:  " + doc.GetElementsByTagName("PlannedStartTime")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned End time of the strip is:  " + doc.GetElementsByTagName("PlannedEndTime")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned duration of the strip is:  " + doc.GetElementsByTagName("PlannedDuration")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Strip ID is:  " + doc.GetElementsByTagName("ID")[4].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Asset ID of the strip is:  " + doc.GetElementsByTagName("ID")[1].InnerText);
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
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            var configFile = currentDir.Replace("bin\\Debug", "extent-config.xml");
            PropertiesCollection.htmlReporter = new ExtentHtmlReporter(@"C:\Report\Report.html");      
            PropertiesCollection.htmlReporter.LoadConfig(configFile);
            PropertiesCollection.extent = new AventStack.ExtentReports.ExtentReports();
            PropertiesCollection.extent.AttachReporter(PropertiesCollection.htmlReporter);
            PropertiesCollection.extent.AddSystemInfo("Automation Database", "8.1");
            PropertiesCollection.extent.AddSystemInfo("Application Under Test (AUT)", "FlightPro API");
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_CreateAssetUnavailability()
        {

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_CreateAssetUnavailability");
            List<String> listDBResponse = new List<string>();

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, "TC001_Asset", strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            strQuery = "select count(*) from dbo.tblstrip";
            command = new SqlCommand(strQuery, myConnection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                intCurStripCount = reader.GetInt32(0);
            }
            reader.Close();

            IRestResponse response = CreateRestRequest("TC001_Asset");
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 200)
            //Success Response
            {
                strQuery = "select count(*) from dbo.tblstrip";
                SqlCommand command1 = new SqlCommand(strQuery, myConnection);
                reader = command1.ExecuteReader();

                int intNewStripCount = 0;

                while (reader.Read())
                {
                    intNewStripCount = reader.GetInt32(0);
                }

                reader.Close();

                try
                {
                    Assert.AreNotEqual(intCurStripCount, intNewStripCount);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for count of Strips is passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for count of Strips is not passed.");
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("PlannedDuration")[0].InnerText, "120");
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Planned duration has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Planned duration has not passed.");
                }
                
                strQuery = "select *from dbo.tblUnavailabilitySubType where UnavailabilitySubTypeID = '" + testData[5] + "' and StripTypeID = 98";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    strUnavailabilityDesc = reader["Description"].ToString(); // Get Unavailability description
                }

                reader.Close();

                strQuery = "select *from tblAsset where AssetID = '" + testData[4] + "'";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    strAssetTypeID = reader["AssetTypeID"].ToString(); // Get Asset Type ID
                    strAssetTail = reader["AssetTail"].ToString(); // Get Asset Tail
                }
                reader.Close();

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("ID")[2].InnerText, strAssetTypeID);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Asset Type ID has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Asset Type ID has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Tail")[0].InnerText, strAssetTail);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Asset Tail has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Asset Tail has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("SupplementalDetails")[0].InnerText, strUnavailabilityDesc);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Unavailaibilty description has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Unavailaibilty description has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("PlannedStartTime")[0].InnerText, strPlannedStartTime);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Planned Start time has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Planned Start time has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("PlannedEndTime")[0].InnerText, strPlannedEndTime);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Planned End time has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Planned End time has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.SelectSingleNode("//Strip/Type").InnerText, "Asset Unavailability");
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Strip type has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Strip type has not passed.");
                }
                try
                {
                    Assert.AreEqual(doc.SelectSingleNode("//Strip/Pane/ID").InnerText, "-1");
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Pane ID has passed");
                }
                catch
                {
                    
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Pane ID has not passed");
                }
            }
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Asset Unavailability couldn't be created. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
        }
        
        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC02_CreateAssetUnavailability_NullAssetID()
        {
            String strTestCaseNo = "TC002_Asset";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC03_CreateAssetUnavailability_NULLSubTypeID()
        {
            String strTestCaseNo = "TC003_Asset";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_CreateAssetUnavailability_NULLStripType()
        {
            String strTestCaseNo = "TC004_Asset";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_CreateAssetUnavailability_InvalidAssetID()
        {
            String strTestCaseNo = "TC005_Asset";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_CreateAssetUnavailability_PlannedStartGreaterThanPlannedEndTime()
        {
            String strTestCaseNo = "TC006_Asset";
            ValidateData(strTestCaseNo);
        }
        
        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
