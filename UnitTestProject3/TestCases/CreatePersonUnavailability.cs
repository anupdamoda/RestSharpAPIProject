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
using System.IO;

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class CreatePersonUnavailability
    {
        const string TblName = "automation_createunavailability";
        const string TestType = "Regression";
        const int SuccessResponse = 200;
        const int BadResponse = 400;
        const string TestCase10 = "TC010_Person";
        const string TestCase11 = "TC011_Person";
        const string TestCase12 = "TC012_Person";
        const string TestCase13 = "TC013_Person";
        const string TestCase14 = "TC014_Person";
        SqlDataReader reader = null;
        SqlCommand command = null;
        String strPlannedStartTime, strPlannedEndTime;

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_CreatePersonUnavailability()
        {
            string query;
            string unavailabilityDesc = string.Empty;
            //long stripCountOld = 0;
            long stripCountNew = 0;

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_CreatePersonUnavailability");

            IRestResponse response = CreateRestRequest(TestCase10);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == SuccessResponse)
            //Success Response
            {
                var connection = new ConnectToMySQL_Fetch_TestData();
                var testData = connection.Select(TblName, TestCase10, TestType);

                //Connecting to application database and retrieving the current count of documents attached to the strip
                string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
                SqlConnection myConnection = new SqlConnection(strConnectionString);
                myConnection.Open();

                query = "select count(*) from dbo.tblstrip s inner join tblStripPeople sp on s.StripID = sp.StripID where s.StripID = " + doc.SelectSingleNode("//Strip/ID").InnerText + " and sp.StripSlot = 1 and sp.PeopleID = " + testData[9];
                command = new SqlCommand(query, myConnection);
                command.CommandType = System.Data.CommandType.Text;
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    stripCountNew = reader.GetInt32(0);
                }

                reader.Close();

                try
                {
                    Assert.AreEqual(1, stripCountNew);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Person Unavailability DB creation has passed");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Person Unavailability DB creation has not passed.");
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

                query = "select * from dbo.tblUnavailabilitySubType where UnavailabilitySubTypeID = '" + testData[5] + "' and StripTypeID = 99";
                command.CommandText = query;
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    unavailabilityDesc = reader["Description"].ToString(); // Get Unavailability description
                }
                reader.Close();

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("SupplementalDetails")[0].InnerText, unavailabilityDesc);
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
                    Assert.AreEqual(doc.SelectSingleNode("//Strip/Type").InnerText, "Person Unavailability");
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
                PropertiesCollection.test.Log(Status.Fail, "Person Unavailability couldn't be created. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC02_CreatePersonUnavailability()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_CreatePersonUnavailability_NoPerson");
            ValidateData(TestCase11);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC03_CreatePersonUnavailability()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_CreatePersonUnavailability_InvalidPerson");
            ValidateData(TestCase12);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_CreatePersonUnavailability()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_CreatePersonUnavailability_NullSubType");
            ValidateData(TestCase13);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_CreatePersonUnavailability()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_CreatePersonUnavailability_InvalidFromDate");
            ValidateData(TestCase14);
        }

        private void ValidateData(string testCase )
        {
            //Posting the Request
            IRestResponse response = CreateRestRequest(testCase);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int responseCode = Get_StatusCode(response);

            if (responseCode == BadResponse)
            {
                try
                {
                    switch (testCase)
                    {
                        case TestCase11:
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP200");
                                //Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "Validation failed. The People is missing or invalid.");
                                break;
                            }
                        case TestCase12:
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");
                                //Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "Validation failed. PeopleID cannot be negative");
                                break;
                            }
                        case TestCase13:
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP200");
                                break;
                            }
                        case TestCase14:
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");
                                break;
                            }

                    }
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Error code has passed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error code received is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "Person Unavailability couldn't be created. Please check your input data");
                    PropertiesCollection.test.Log(Status.Pass, "Error Message received is: " + doc.GetElementsByTagName("Message")[0].InnerText);

                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Error code has failed.");
                    PropertiesCollection.test.Log(Status.Fail, "Error Code is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Fail, "Error message received is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
            }
            else if (responseCode == SuccessResponse)
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Person unavailability has been created with wrong input data.");
                PropertiesCollection.test.Log(Status.Fail, "Strip ID created is: " + doc.SelectSingleNode("//Strip/ID").InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Strip type created is: " + doc.GetElementsByTagName("Type")[2].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned Start time of the strip is:  " + doc.GetElementsByTagName("PlannedStartTime")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned End time of the strip is:  " + doc.GetElementsByTagName("PlannedEndTime")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Planned duration of the strip is:  " + doc.GetElementsByTagName("PlannedDuration")[0].InnerText);
               
            }
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Please check your input");
                PropertiesCollection.test.Log(Status.Fail, "Error code is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }

        }

        private int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }


        private IRestResponse CreateRestRequest(string testCaseNo)
        {
            DateTime plannedStartTime, plannedEndTime;

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(TblName, testCaseNo, TestType);

            plannedStartTime = DateTime.UtcNow;
            if (testCaseNo != TestCase14)
                plannedEndTime = plannedStartTime.AddHours(2);
            else
                plannedEndTime = plannedStartTime.AddHours(-1);

            strPlannedStartTime = plannedStartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            strPlannedEndTime = plannedEndTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            strPlannedStartTime = strPlannedStartTime.Remove(14, 2);
            strPlannedStartTime = strPlannedStartTime.Insert(14, "00");

            strPlannedEndTime = strPlannedEndTime.Remove(14, 2);
            strPlannedEndTime = strPlannedEndTime.Insert(14, "00");

            var client = new RestClient(ConfigurationManager.AppSettings["InsertStripEndPoint"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["InsertStripResource"], Method.POST);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            ConstructRequestBody(request, testCaseNo, testData, strPlannedStartTime, strPlannedEndTime);

            //Triggering the Request
            IRestResponse response = client.Execute(request);

            return response;
        }

        private void ConstructRequestBody(RestRequest request, string testCaseNo, string[] testData, string plannedStartTime, string plannedEndTime)
        {
            string requestBody;
            
            switch (testCaseNo)
            {
                case TestCase10:
                case TestCase12:
                case TestCase14:
                    {
                        requestBody = $"<?xml version=\"1.0\"?>\n<Strip>\n  <SubTypeID> {testData[5]} </SubTypeID>\n <PlannedStartTime> {plannedStartTime} </PlannedStartTime>\n  <PlannedEndTime> {plannedEndTime} </PlannedEndTime>\n <People>\n <StripPerson> \n<PersonID>{testData[9]}</PersonID> \n</StripPerson>\n </People> \n <Type>{testData[8]}</Type>\n</Strip>";
                                    
                        request.AddParameter("undefined", requestBody, ParameterType.RequestBody);
                        
                        break;
                    }
                case TestCase11:
                    {
                        requestBody = $"<?xml version=\"1.0\"?>\n<Strip>\n  <SubTypeID> {testData[5]} </SubTypeID>\n <PlannedStartTime> {plannedStartTime} </PlannedStartTime>\n  <PlannedEndTime> {plannedEndTime} </PlannedEndTime>\n <Type>{testData[8]}</Type>\n</Strip>";

                        request.AddParameter("undefined", requestBody, ParameterType.RequestBody);

                        break;
                    }
                case TestCase13:
                    {
                        requestBody = $"<?xml version=\"1.0\"?>\n<Strip>\n <PlannedStartTime> {plannedStartTime} </PlannedStartTime>\n  <PlannedEndTime> {plannedEndTime} </PlannedEndTime>\n <Type>{testData[8]}</Type>\n</Strip>";

                        request.AddParameter("undefined", requestBody, ParameterType.RequestBody);

                        break;
                    }
            }
        }

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
        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
