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
    public class InsertWayPoint
    {
        String strtblname = "automation_waypoint";
        String strTestType = "Regression";
        int intWaypointCount;

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["InsertStripDocumentEndPoint"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["InsertWayPointEndResource"], Method.POST);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            
            switch (TestCaseNo)
            {
                case "TC001":
                    //Adding XML Body
                    request.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Colour>" + testData[4] +"</Colour>\r\n  <DisplayCode>" + testData[5] + "</DisplayCode>\r\n  <Latitude>" + testData[6] + "</Latitude>\r\n  <Longitude>" + testData[7] + "</Longitude>\r\n  <Name>" + testData[8] + "</Name>\r\n  <ShortCode>" + testData[9] + "</ShortCode>\r\n  <TimeZoneKey>" + testData[10] + "</TimeZoneKey>\r\n  <UseDaylightSavings>" + testData[11] + "</UseDaylightSavings>\r\n</Waypoint>\r\n", ParameterType.RequestBody);
                    break;
                case "TC002":
                    request.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Name>" + testData[8] + "</Name>\r\n  <ShortCode>" + testData[9] + "</ShortCode>\r\n  <TimeZoneKey>" + testData[10] + "</TimeZoneKey>\r\n</Waypoint>\r\n", ParameterType.RequestBody);
                    break;

                case "TC003":
                    request.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Name>" + testData[8] + "</Name>\r\n  <ShortCode>" + testData[9] + "</ShortCode>\r\n  <TimeZoneKey>" + testData[10] + "</TimeZoneKey>\r\n</Waypoint>\r\n", ParameterType.RequestBody);
                    break;

                case "TC004":
                    request.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Name>" + testData[8] + "</Name>\r\n  <ShortCode>" + testData[9] + "</ShortCode>\r\n  <TimeZoneKey>" + testData[10] + "</TimeZoneKey>\r\n</Waypoint>\r\n", ParameterType.RequestBody);
                    break;

            }


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
                case "TC002":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_InsertWaypoint_VerifyDefaultedValues");
                    break;
                case "TC003":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_InsertWaypoint_VerifyErrorResponses");
                    break;
                case "TC004":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_InsertWaypoint_VerifyErrorResponses2");
                    break;

            }

            //Posting the Request
            IRestResponse response = CreateRestRequest(strTestCaseNo);
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            Console.WriteLine("Status Code" + intRespCode);

            if (intRespCode == 400)
            {
                try
                {
                    if (strTestCaseNo == "TC003" || strTestCaseNo == "TC004" || strTestCaseNo == "TC005" )
                    {

                          Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP200");
                            

                        if (strTestCaseNo == "TC003" )
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The ShortCode is missing or invalid.");
                                PropertiesCollection.test.Log(Status.Pass,"Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed" );
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail,"Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                        if (strTestCaseNo == "TC004" )
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The Name is missing or invalid.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }

                        }

                        if (strTestCaseNo == "TC005")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The TimeZoneKey is missing or invalid.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                    }
                    else if (strTestCaseNo == "TC003" || strTestCaseNo == "TC005" || strTestCaseNo == "TC006")
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

            /*
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
            */
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

        [Priority(1)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_InsertWayPoint()
        {

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_InsertWayPoint");
            Object[] ObjDBResponse = new object[16];

            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, "TC001", strTestType);

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            SqlDataReader reader = null;
            String strQuery = "select count(*) from dbo.tblwaypoint where WaypointName = '" + testData[8] + "' and WaypointShortCode= '" + testData[9] + "'";
            SqlCommand command = new SqlCommand(strQuery, myConnection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                intWaypointCount = reader.GetInt32(0);
            }
            reader.Close();

            IRestResponse response = CreateRestRequest("TC001");
            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            //Retrieving & printing the Response code
            int intRespCode = Get_StatusCode(response);

            if (intRespCode == 200)
            //Success Response
            {
                
                strQuery = "select count(*) from dbo.tblwaypoint where WaypointName = '" + testData[8] + "' and WaypointShortCode= '" + testData[9] + "'";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                int newWaypointCount = 0;

                while (reader.Read())
                {
                    newWaypointCount = reader.GetInt32(0);
                }

                reader.Close();

                try
                {
                    Assert.AreEqual(intWaypointCount+1, newWaypointCount);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for count of Waypoint is passed");

                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for count of Waypoint is not passed.");
                }

                strQuery = "select * from dbo.tblwaypoint where WaypointName = '" + testData[8] + "' and WaypointShortCode= '" + testData[9] + "'";
                command = new SqlCommand(strQuery, myConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ObjDBResponse[0] = reader[0].ToString(); //WayPointID
                    ObjDBResponse[1] = reader[1].ToString(); //WayPointColour
                    ObjDBResponse[2] = reader[3].ToString(); //WayPointName
                    ObjDBResponse[3] = reader[4].ToString(); //WaypointShortCode
                    ObjDBResponse[4] = reader[7].ToString(); //TimeZoneKey
                    ObjDBResponse[5] = reader[10].ToString(); //LocationLatitude
                    ObjDBResponse[6] = reader[11].ToString(); //LocationLongitude
                    ObjDBResponse[7] = reader[13].ToString(); //WayPointDisplayCode
                    ObjDBResponse[8] = reader[15].ToString(); //LoginName
                }

                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("Colour")[0].InnerText, testData[4]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Colour is passed and Waypoint Colour is "+ ObjDBResponse[1]);
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Colour is not passed");
                }
                try
                {
                    Assert.AreEqual(doc.GetElementsByTagName("DisplayCode")[0].InnerText, testData[5]);
                    PropertiesCollection.test.Log(Status.Pass, "Validation for DisplayCode is passed and is " + ObjDBResponse[7]);

                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for DisplayCode is not passed");
                }
                try
                {
                    Assert.AreEqual(ObjDBResponse[8], "AFMIS");
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Login name is passed and is "+ ObjDBResponse[8]);

                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Login name is not passed. Expected is AFMIS. Actual is: " + ObjDBResponse[8]);
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
                PropertiesCollection.test.Log(Status.Fail, "Insert Waypoint API not passed. Please check your input parameters");
                PropertiesCollection.test.Log(Status.Fail, "Error Code received is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }

        }

        

        [Priority(2)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC02_InsertWaypoint_VerifyDefaultedValues()
        {
            String strTestCaseNo = "TC002";
            ValidateData(strTestCaseNo);
        }

        [Priority(3)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC03_InsertWaypoint_ShortCodeMissing()
        {
            String strTestCaseNo = "TC003";
            ValidateData(strTestCaseNo);
        }

        [Priority(4)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_InsertWaypoint_TimeZoneMissing()
        {
            String strTestCaseNo = "TC004";
            ValidateData(strTestCaseNo);
        }

        [Priority(5)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_InsertWaypoint_NameMissing()
        {
            String strTestCaseNo = "TC005";
            ValidateData(strTestCaseNo);
        }

        [Priority(6)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_InsertWaypoint_ShortCodeInvalidData()
        {
            String strTestCaseNo = "TC006";
            ValidateData(strTestCaseNo);
        }

        [Priority(7)]
        [TestCategory("Regression")]
        [TestMethod]
        public void TC07_InsertWaypoint_VerifyErrorResponse()
        {
            String strTestCaseNo = "TC007";
            ValidateData(strTestCaseNo);
        }


        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }
    }
}
