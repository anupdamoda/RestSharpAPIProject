using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using RestSharp;
using FPWebAutomation_MSTests;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using UnitTestProject3.Database;
using System.IO;

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class SearchWaypoint
    {

        String strtblname = "automation_waypoint";
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
        public void TC01_SearchWaypoint_Equals()
        {
            var TestCaseNo = "TC01"; 
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_SearchWaypoint_Equals");
            

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value>" + testData[13] + "</Value>\r\n        <SearchMode>Equals</SearchMode>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

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
                Assert.AreEqual(doc.GetElementsByTagName("ShortCode")[0].InnerText, testData[13]);                
                PropertiesCollection.test.Log(Status.Pass, "Short Code displayed is as per the search criteria as " + testData[13]);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Short Code displayed is not as per the search criteria");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("//[contains(.,'Locations')]"));
                PropertiesCollection.test.Log(Status.Pass, "Locations tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Locations tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Location")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Location tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Location tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsHistorical")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsHistorical tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsHistorical tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdated")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdatedBy")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Latitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Latitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Latitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Longitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Longitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Longitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ShortCode")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ShortCode tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ShortCode tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TimeZoneKey")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TimeZoneKey tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TimeZoneKey tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UseDayLightSavings")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UseDayLightSavings tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UseDayLightSavings tag is not present");
            }
        }

        [TestMethod]
        public void TC02_SearchWaypoint_StartsWith()
        {
            var TestCaseNo = "TC02";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_SearchWaypoint_StartsWith");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value>" + testData[13] + "</Value>\r\n        <SearchMode>StartsWith</SearchMode>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

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
                Assert.AreEqual(doc.GetElementsByTagName("ShortCode")[0].InnerText.Substring(0,3), testData[13]);
                PropertiesCollection.test.Log(Status.Pass, "Short Code displayed is as per the search criteria as " + doc.GetElementsByTagName("ShortCode")[0].InnerText);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Short Code displayed is not as per the search criteria");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("//[contains(.,'Locations')]"));
                PropertiesCollection.test.Log(Status.Pass, "Locations tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Locations tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Location")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Location tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Location tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsHistorical")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsHistorical tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsHistorical tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdated")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdatedBy")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Latitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Latitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Latitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Longitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Longitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Longitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ShortCode")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ShortCode tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ShortCode tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TimeZoneKey")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TimeZoneKey tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TimeZoneKey tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UseDayLightSavings")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UseDayLightSavings tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UseDayLightSavings tag is not present");
            }
        }

        [TestMethod]
        public void TC03_SearchWaypoint_Contains()
        {
            var TestCaseNo = "TC03";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_SearchWaypoint_Contains");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value>" + testData[13] + "</Value>\r\n        <SearchMode>Contains</SearchMode>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

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
                Assert.AreEqual(doc.GetElementsByTagName("ShortCode")[0].InnerText.Substring(1, 2), testData[13]);
                PropertiesCollection.test.Log(Status.Pass, "Short Code displayed is as per the search criteria as " + doc.GetElementsByTagName("ShortCode")[0].InnerText);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Short Code displayed is not as per the search criteria");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("//[contains(.,'Locations')]"));
                PropertiesCollection.test.Log(Status.Pass, "Locations tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Locations tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Location")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Location tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Location tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsHistorical")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsHistorical tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsHistorical tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdated")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdatedBy")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Latitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Latitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Latitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Longitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Longitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Longitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ShortCode")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ShortCode tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ShortCode tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TimeZoneKey")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TimeZoneKey tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TimeZoneKey tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UseDayLightSavings")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UseDayLightSavings tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UseDayLightSavings tag is not present");
            }
        }

        [TestMethod]
        public void TC04_SearchWaypoint_MaxMinIntegers()
        {
            var TestCaseNo = "TC04";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_SearchWaypoint_MaxMinIntegers");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ID>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Minimum>" + testData[15] + "</Minimum>\r\n        <Maximum>" + testData[14] + "</Maximum>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ID>\r\n</Waypoint>", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

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
                Assert.IsTrue(Convert.ToInt64(doc.GetElementsByTagName("ID")[0].InnerText) >= Convert.ToInt64(testData[15]) && Convert.ToInt64(doc.GetElementsByTagName("ID")[0].InnerText) <= Convert.ToInt64(testData[14]));            
                PropertiesCollection.test.Log(Status.Pass, "ID displayed is within the search criteria as " + doc.GetElementsByTagName("ID")[0].InnerText);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID displayed is not within the search criteria");
            }
          
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("//[contains(.,'Locations')]"));
                PropertiesCollection.test.Log(Status.Pass, "Locations tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Locations tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Location")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Location tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Location tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsHistorical")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsHistorical tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsHistorical tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdated")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdatedBy")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Latitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Latitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Latitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Longitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Longitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Longitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ShortCode")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ShortCode tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ShortCode tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TimeZoneKey")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TimeZoneKey tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TimeZoneKey tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UseDayLightSavings")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UseDayLightSavings tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UseDayLightSavings tag is not present");
            }
        }

        [TestMethod]
        public void TC05_SearchWaypoint_MaxMinDate()
        {
            var TestCaseNo = "TC05";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_SearchWaypoint_MaxMinDate");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\n  <FullResponse>true</FullResponse>\n  <LastUpdated>\n    <Filters>\n      <FilterItem>\n        <Minimum>" + testData[15] + "</Minimum>\n        <Maximum>" + testData[14] + "</Maximum>\n      </FilterItem>\n    </Filters>\n  </LastUpdated>\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            Console.WriteLine(response.Content);

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
                Assert.IsTrue(Convert.ToDateTime(doc.GetElementsByTagName("LastUpdated")[0].InnerText) >= Convert.ToDateTime(testData[15]) && Convert.ToDateTime(doc.GetElementsByTagName("LastUpdated")[0].InnerText) <= Convert.ToDateTime(testData[14]));
                PropertiesCollection.test.Log(Status.Pass, "Date displayed is within the search criteria as " + doc.GetElementsByTagName("LastUpdated")[0].InnerText);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Date displayed is not within the search criteria");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("//[contains(.,'Locations')]"));
                PropertiesCollection.test.Log(Status.Pass, "Locations tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Locations tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Location")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Location tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Location tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ID tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsHistorical")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsHistorical tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsHistorical tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdated")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("LastUpdatedBy")[0]);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Latitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Latitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Latitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Longitude")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Longitude tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Longitude tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ShortCode")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ShortCode tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ShortCode tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TimeZoneKey")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TimeZoneKey tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TimeZoneKey tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UseDayLightSavings")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UseDayLightSavings tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UseDayLightSavings tag is not present");
            }
        }

        [TestMethod]
        public void TC06_SearchWaypoint_NoSearchCriteria()
        {
            var TestCaseNo = "TC06";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_SearchWaypoint_NoSearchCriteria");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>    \r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText;
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
                Assert.AreEqual(Code, testData[17]);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, testData[16]);
                PropertiesCollection.test.Log(Status.Pass, "Message is " + Message);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Message is " + Message);
            }
        }

             [TestMethod]
        public void TC07_SearchWaypoint_NoReturnfields()
        {
            var TestCaseNo = "TC07";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC07_SearchWaypoint_NoReturnfields");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\n  <ID>\n    <IncludeInResponse>false</IncludeInResponse>\n    <Filters>\n      <FilterItem>\n        <Value>102</Value>\n      </FilterItem>\n    </Filters>\n  </ID>\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText;
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
                Assert.AreEqual(Code, testData[17]);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, testData[16]);
                PropertiesCollection.test.Log(Status.Pass, "Message is " + Message);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Message is " + Message);
            }
        }

        [TestMethod]
        public void TC08_SearchWaypoint_InvalidSearchMode()
        {
            var TestCaseNo = "TC08";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC08_SearchWaypoint_InvalidSearchMode");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value></Value>\r\n        <SearchMode></SearchMode>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText.Substring(0,31);
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
                Assert.AreEqual(Code, testData[17]);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, testData[16]);
                PropertiesCollection.test.Log(Status.Pass, "Message is " + Message);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Message is " + Message);
            }
        }

        [TestMethod]
        public void TC09_SearchWaypoint_InvalidDataFormat()
        {
            var TestCaseNo = "TC09";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC09_SearchWaypoint_InvalidDataFormat");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n\t<FullResponse>true</FullResponse>\r\n  <ID>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value>abcd</Value>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ID>\r\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText;
            Console.WriteLine(Message);

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
                Assert.AreEqual(Code, testData[17]);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, testData[16]);
                PropertiesCollection.test.Log(Status.Pass, "Message is " + Message);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Message is " + Message);
            }
        }

        [TestMethod]
        public void TC10_SearchWaypoint_NoMatchingResult()
        {
            var TestCaseNo = "TC10";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC10_SearchWaypoint_NoMatchingResult");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("locations/Search/", Method.POST);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Adding XML Body

            request.AddParameter("undefined", "<Waypoint>\r\n  <FullResponse>true</FullResponse>\r\n  <ShortCode>\r\n    <Filters>\r\n      <FilterItem>\r\n        <Value>XYZ</Value>\r\n        <SearchMode>Contains</SearchMode>\r\n      </FilterItem>\r\n    </Filters>\r\n  </ShortCode>\r\n</Waypoint>", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
                                 
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
                Assert.AreEqual(response.Content, testData[16]);
                PropertiesCollection.test.Log(Status.Pass, "Response is valid");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Response is invalid");
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
