using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FPWebAutomation_MSTests;
using UnitTestProject3.Database;
using System.Data.SqlClient;
using System.Configuration;
using RestSharp;
using AventStack.ExtentReports;
using System.Xml;
using AventStack.ExtentReports.Reporter;
using System.Net;

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class UpdateWaypoint
    {
        String strtblname = "automation_waypoint";
        String strTestType = "Regression";

        public int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }

        int intWaypointCount;

       
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_UpdateWaypoint_waypoint()
        {

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_UpdateWaypoint");
            Object[] ObjDBResponse = new object[16];

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            SqlDataReader reader = null;

            /****************** Retriving the count of Waypoint before Insert **************************************/
            String strQuerybefore = "select count(*) from dbo.tblWaypoint";

            SqlCommand commandbefore = new SqlCommand(strQuerybefore, myConnection);
            reader = commandbefore.ExecuteReader();

            while (reader.Read())
            {
                intWaypointCount = reader.GetInt32(0);
            }
            reader.Close();

            intWaypointCount = intWaypointCount - 1;
            Console.WriteLine("Count of Waypoints in Application Database: " + intWaypointCount);

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);

            /**** PREREQUISITE *** Insert  Waypoint Operation and inserting the Waypoint ID into the Update Row *****/



            /****************** TestData Database call to retrieve the Insert details  ******************************/

            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, "TC001", strTestType);

            
            var InsertRequest = new RestRequest(ConfigurationManager.AppSettings["InsertWayPointEndResource"], Method.POST);
            InsertRequest.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            InsertRequest.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            InsertRequest.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            InsertRequest.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
            InsertRequest.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Colour>" + testData[4] + "</Colour>\r\n  <DisplayCode>" + testData[5] + "</DisplayCode>\r\n  <Latitude>" + testData[6] + "</Latitude>\r\n  <Longitude>" + testData[7] + "</Longitude>\r\n  <Name>" + testData[8] + "</Name>\r\n  <ShortCode>" + testData[9] + "</ShortCode>\r\n  <TimeZoneKey>" + testData[10] + "</TimeZoneKey>\r\n  <UseDaylightSavings>" + testData[11] + "</UseDaylightSavings>\r\n</Waypoint>\r\n", ParameterType.RequestBody);

            IRestResponse response = client.Execute(InsertRequest);
            var content = client.Execute(InsertRequest).Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            String WaypointID = doc.GetElementsByTagName("WaypointID")[0].InnerText;
             
            Console.WriteLine("WaypointID: " + WaypointID);

            var updateconnection = new ConnectToMySQL_Update_TestData();
            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC005");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC006");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC007");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC008");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC009");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC010");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC011");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC013");

            updateconnection.Update("automation_waypoint", "WaypointID", WaypointID, "TC014");


            /****************** Retriving the count of Waypoint before update **************************************/
            String strQuery = "select count(*) from dbo.tblWaypoint";

            SqlCommand command = new SqlCommand(strQuery, myConnection);
            reader = command.ExecuteReader();


            while (reader.Read())
            {
                intWaypointCount = reader.GetInt32(0);
            }
            reader.Close();

            intWaypointCount= intWaypointCount - 1;
            Console.WriteLine("Count of Waypoints in Application Database: " + intWaypointCount);

            /****************** TestData Database call to retrieve the Update Testcase details  ********************/

            var connectionupdate = new ConnectToMySQL_Fetch_TestData();
            var testDataupdate = connection.Select(strtblname, "TC005", strTestType);



            /****************** UpdateStripDoc Waypoint Operation  *************************************************/


            var UpdateRequest = new RestRequest(ConfigurationManager.AppSettings["InsertWayPointEndResource"]+ testDataupdate[12], Method.PUT);

            //Adding headers to the PUT request

            UpdateRequest.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            UpdateRequest.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            UpdateRequest.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            UpdateRequest.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
            UpdateRequest.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Name>"+ testDataupdate[8] +"</Name>\r\n  <ShortCode>" + testDataupdate[9] +  "</ShortCode>\r\n  <TimeZoneKey>" + testDataupdate[10] + "</TimeZoneKey>\r\n</Waypoint>", ParameterType.RequestBody);

            IRestResponse Updateresponse = client.Execute(UpdateRequest);
            var Updatecontent = client.Execute(UpdateRequest).Content;

            Console.WriteLine("Content Length: " + content.Length);
            Console.WriteLine("Response Status: " + response.ResponseStatus);

            Console.WriteLine("Status Code: " + response.StatusCode);
            Console.WriteLine("Header Count: " + response.Headers.Count);

            var docUpdate = new XmlDocument();
            docUpdate.LoadXml(Updateresponse.Content);

            /******************************************************************************************************/

            /****************** Retriving the count of Waypoint after update **************************************/
            String strQueryafter = "select count(*) from dbo.tblWaypoint";

            SqlCommand commandafter = new SqlCommand(strQueryafter, myConnection);
            reader = commandafter.ExecuteReader();


            while (reader.Read())
            {
                intWaypointCount = reader.GetInt32(0);
            }
            reader.Close();

            intWaypointCount = intWaypointCount - 1;
            Console.WriteLine("Count of Waypoints in Application Database: " + intWaypointCount);
            /****************** Retriving the count of Waypoint after update **************************************/

            int intRespCode = Get_StatusCode(response);
          
            try
            {
                Console.WriteLine("intResponse: " + intRespCode);
                Assert.AreEqual(intRespCode,200);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
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
                PropertiesCollection.test.Log(Status.Pass, "Validation for Colour is passed and Waypoint Colour is " + ObjDBResponse[1]);
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
                PropertiesCollection.test.Log(Status.Pass, "Validation for Login name is passed and is " + ObjDBResponse[8]);

            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Login name is not passed. Expected is AFMIS. Actual is: " + ObjDBResponse[8]);
            }
            
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_UpdateWaypoint_VerifyErrRes_InvalidDisplayCode()
        {
            String strTestCaseNo = "TC006";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC07_UpdateWaypoint_VerifyErrRes_InvalidShortCode()
        {
            String strTestCaseNo = "TC007";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC08_UpdateWaypoint_VerifyErrRes_InvalidTimesZoneKey()
        {
            String strTestCaseNo = "TC008";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC09_UpdateWaypoint_VerifyErrRes_InvalidUseDaylightSavings()
        {
            String strTestCaseNo = "TC009";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC10_UpdateWaypoint_VerifyErrRes_InvalidLatitude()
        {
            String strTestCaseNo = "TC010";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC11_UpdateWaypoint_VerifyErrRes_InvalidLongitude()
        {
            String strTestCaseNo = "TC011";
            ValidateData(strTestCaseNo);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void TC12_UpdateWaypoint_VerifyErrRes_InvalidWaypointID()
        {
            String strTestCaseNo = "TC012";
            ValidateData(strTestCaseNo);
        }

        //Function for validating invalid inputs -- Negative Scenarios
        public void ValidateData(String strTestCaseNo)
        {
            switch (strTestCaseNo)
            {
                case "TC006":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_UpdateWaypoint_VerifyErrRes_InvalidDisplayCode");
                    break;
                case "TC007":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC07_UpdateWaypoint_VerifyErrRes_InvalidShortCode");
                    break;
                case "TC008":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC08_UpdateWaypoint_VerifyErrRes_InvalidTimesZoneKey");
                    break;
                case "TC009":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC09_UpdateWaypoint_VerifyErrRes_InvalidUseDaylightSavings");
                    break;
                case "TC010":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC10_UpdateWaypoint_VerifyErrRes_InvalidLatitude");
                    break;
                case "TC011":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC11_UpdateWaypoint_VerifyErrRes_InvalidLongitude");
                    break;
                case "TC012":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC12_UpdateWaypoint_VerifyErrRes_InvalidWaypointID");
                    break;
                case "TC013":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC13_UpdateWaypoint_VerifyErrRes_InvalidIsHistorical");
                    break;
                case "TC014":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC14_UpdateWaypoint_VerifyErrRes_InvalidLocation");
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
                    if (strTestCaseNo == "TC006" || strTestCaseNo == "TC007" ||  strTestCaseNo == "TC009" || strTestCaseNo == "TC013" || strTestCaseNo == "TC014")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP105");

                        if (strTestCaseNo == "TC006")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The Input is invalid in format. Details: The field DisplayCode must be a string with a maximum length of 4.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                        if (strTestCaseNo == "TC007")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The Input is invalid in format. Details: The field ShortCode must be a string with a maximum length of 4.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }

                        }

                        if (strTestCaseNo == "TC009")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The TimeZoneKey 'Australia Time' is invalid.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                        if (strTestCaseNo == "TC013")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The Input is invalid in format. Details: There was an error deserializing the object of type Ocean.FlightPro.API.Models.LocationInputDTO. The value 'TEST' cannot be parsed as the type 'Boolean'.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                        if (strTestCaseNo == "TC014")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "The Input is invalid in format. Details: The field Name must be a string with a maximum length of 50.");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                    }
                    else if (strTestCaseNo == "TC010" || strTestCaseNo == "TC011" )
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP202");

                        if (strTestCaseNo == "TC010")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "Input provided for Latitude can not be converted to degrees, minutes and seconds");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }
                        }

                        if (strTestCaseNo == "TC011")
                        {
                            try
                            {
                                Assert.AreEqual(doc.GetElementsByTagName("Message")[0].InnerText, "Input provided for Longitude can not be converted to degrees, minutes and seconds");
                                PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Passed");
                            }
                            catch
                            {
                                PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Message")[0].InnerText + " Failed");
                            }

                        }



                    }
                    else if (strTestCaseNo == "TC012")
                    {
                        try
                        {
                            Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP101");
                            PropertiesCollection.test.Log(Status.Pass, "Negative Scenario: " + doc.GetElementsByTagName("Code")[0].InnerText + "Passed");
                        }
                        catch
                        {
                            PropertiesCollection.test.Log(Status.Fail, "Negative Scenario: " + doc.GetElementsByTagName("Code")[0].InnerText + "Failed");
                        }


                    }
                    PropertiesCollection.test.Log(Status.Pass, "Validation for Error code has passed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error code received is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Pass, "The requested resource is not allowed.");
                    PropertiesCollection.test.Log(Status.Pass, "Error Message received is: " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Validation for Error code has failed.");
                    PropertiesCollection.test.Log(Status.Fail, "Error Code is:  " + doc.GetElementsByTagName("Code")[0].InnerText);
                    PropertiesCollection.test.Log(Status.Fail, "Error message received is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
                }
            }
            
            else
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Please check your input");
                PropertiesCollection.test.Log(Status.Fail, "Error code is: " + doc.GetElementsByTagName("Code")[0].InnerText);
                PropertiesCollection.test.Log(Status.Fail, "Error Response is:  " + doc.GetElementsByTagName("Message")[0].InnerText);
            }
        }

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testDataupdate = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var UpdateRequest = new RestRequest(ConfigurationManager.AppSettings["InsertWayPointEndResource"] + testDataupdate[12], Method.PUT);

            //Adding headers to the POST request
            UpdateRequest.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            UpdateRequest.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            UpdateRequest.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            UpdateRequest.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
            UpdateRequest.AddParameter("undefined", "<?xml version=\"1.0\"?>\r\n<Waypoint>\r\n  <Colour>"+ testDataupdate[4] +"</Colour>\r\n  <DisplayCode>"+ testDataupdate[5] + "</DisplayCode>\r\n  <Latitude>"+ testDataupdate[6] + "</Latitude>\r\n  <Longitude>" + testDataupdate[7] + "</Longitude>\r\n  <Name>" + testDataupdate[8] + "</Name>\r\n  <ShortCode>"+ testDataupdate[9] +"</ShortCode>\r\n  <TimeZoneKey>" + testDataupdate[10] + "</TimeZoneKey>\r\n  <UseDaylightSavings>" + testDataupdate[11] + "</UseDaylightSavings>\r\n</Waypoint>", ParameterType.RequestBody);

            //Posting the Request
            IRestResponse response = client.Execute(UpdateRequest);

            return response;
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


        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }


    }


    }
 

