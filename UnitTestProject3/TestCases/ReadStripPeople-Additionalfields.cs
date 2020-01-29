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

namespace UnitTestProject3.TestCases
{
    [TestClass]
    public class ReadStripPeopleAdditionalfields
    {

        String strtblname = "automation_StripPeople";
        String strTestType = "Regression";

        public int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }

        [TestMethod]
        public void TC01_ReadStrip_SinglePersonnel()
        {
            var TestCaseNo = "TC001"; 
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStrip_SinglePersonnel");
            

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["ReadStripPeople"]+ testData[4], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = client.Execute(request).Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            int intRespCode = Get_StatusCode(response);


            var AssessorId = doc.GetElementsByTagName("AssessorID")[0].InnerText;
            var CallSign = doc.GetElementsByTagName("Callsign")[0].InnerText;
            Boolean GivenNames = doc.GetElementsByTagName("GivenNames")[0].HasChildNodes;
            Boolean IndividualTask = doc.GetElementsByTagName("IndividualTask")[0].HasChildNodes;

            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

            try
            {
                Assert.IsNotNull(AssessorId);
                PropertiesCollection.test.Log(Status.Pass, "AssessorId value is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssessorId value is not present");
            }

            try
            {
                Assert.IsNotNull(CallSign);
                PropertiesCollection.test.Log(Status.Pass, "CallSign value is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CallSign value is not present");
            }


            try
            {
                Assert.IsNotNull(GivenNames);
                PropertiesCollection.test.Log(Status.Pass, "GivenNames is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "GivenNames is not present");
            }

            try
            {
                Assert.IsNotNull(IndividualTask);
                PropertiesCollection.test.Log(Status.Pass, "IndividualTask is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IndividualTask is not present");
            }
        }

        [TestMethod]
        public void TC02_ReadStrip_MulitplePersonnel()
        {
            var TestCaseNo = "TC002";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_ReadStrip_MulitplePersonnel");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["ReadStripPeople"] + testData[4], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = client.Execute(request).Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            int intRespCode = Get_StatusCode(response);

            /*******************Validation for First Personnel************************************************/

            var AssessorId_FirstPersonnel = doc.GetElementsByTagName("AssessorID")[0].InnerText;
            var CallSign_FirstPersonnel = doc.GetElementsByTagName("Callsign")[0].InnerText;
            Boolean GivenNames_FirstPersonnel = doc.GetElementsByTagName("GivenNames")[0].HasChildNodes;
            Boolean IndividualTask_FirstPersonnel = doc.GetElementsByTagName("IndividualTask")[0].HasChildNodes; 


            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }


            try
            {
                Assert.IsNotNull(AssessorId_FirstPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "AssessorId value for the first personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssessorId value for the first personnel is not present");
            }

            try
            {
                Assert.IsNotNull(CallSign_FirstPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "CallSign value for the first personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CallSign value for the first personnel is not present");
            }


            try
            {
                Assert.IsNotNull(GivenNames_FirstPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "GivenNames for the first personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "GivenNames for the first personnel is not present");
            }

            try
            {
                Assert.IsNotNull(IndividualTask_FirstPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "IndividualTask for the first personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IndividualTask for the first personnel is not present");
            }

            /*******************Validation for Second Personnel************************************************/

            var AssessorId_SecondPersonnel = doc.GetElementsByTagName("AssessorID")[1].InnerText;
            var CallSign_SecondPersonnel = doc.GetElementsByTagName("Callsign")[1].InnerText;
            Boolean GivenNames_SecondPersonnel = doc.GetElementsByTagName("GivenNames")[1].HasChildNodes;
            Boolean IndividualTask_SecondPersonnel = doc.GetElementsByTagName("IndividualTask")[1].HasChildNodes;


            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

            try
            {
                Assert.IsNotNull(AssessorId_SecondPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "AssessorId value for the Second personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssessorId value for the Second personnel is not present");
            }

            try
            {
                Assert.IsNotNull(CallSign_SecondPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "CallSign value for the Second Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CallSign value for the Second Personnel is not present");
            }

            try
            {
                Assert.IsNotNull(GivenNames_SecondPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "GivenNames values for the Second Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "GivenNames values for the Second Personnel is not present");
            }

            try
            {
                Assert.IsNotNull(IndividualTask_FirstPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "IndividualTask for the Second Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IndividualTask for the Second Personnel is not present");
            }

            /*******************Validation for Third Personnel************************************************/

            var AssessorId_ThirdPersonnel = doc.GetElementsByTagName("AssessorID")[2].InnerText;
            var CallSign_ThirdPersonnel = doc.GetElementsByTagName("Callsign")[2].InnerText;
            Boolean GivenNames_ThirdPersonnel = doc.GetElementsByTagName("GivenNames")[2].HasChildNodes;
            Boolean IndividualTask_ThirdPersonnel = doc.GetElementsByTagName("IndividualTask")[2].HasChildNodes;


            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }


            try
            {
                Assert.IsNotNull(AssessorId_ThirdPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "AssessorId value for the Third personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssessorId value for the Third personnel is not present");
            }

            try
            {
                Assert.IsNotNull(CallSign_ThirdPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "CallSign value for the Third Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CallSign value for the Third Personnel is not present");
            }


            try
            {
                Assert.IsNotNull(GivenNames_ThirdPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "GivenNames values for the Third Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "GivenNames values for the Third Personnel is not present");
            }

            try
            {
                Assert.IsNotNull(IndividualTask_ThirdPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "IndividualTask for the Third Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IndividualTask for the Third Personnel is not present");
            }


            /*******************Validation for Fourth / French Personnel************************************************/

            var AssessorId_FrenchPersonnel = doc.GetElementsByTagName("AssessorID")[3].InnerText;
            var CallSign_FrenchPersonnel = doc.GetElementsByTagName("Callsign")[3].InnerText;
            Boolean GivenNames_FrenchPersonnel = doc.GetElementsByTagName("GivenNames")[3].HasChildNodes;
            Boolean IndividualTask_FrenchPersonnel = doc.GetElementsByTagName("IndividualTask")[3].HasChildNodes;


            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }


            try
            {
                Assert.IsNotNull(AssessorId_FrenchPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "AssessorId value for the French personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssessorId value for the French personnel is not present");
            }

            try
            {
                Assert.IsNotNull(CallSign_FrenchPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "CallSign value for the French Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CallSign value for the French Personnel is not present");
            }


            try
            {
                Assert.IsNotNull(GivenNames_FrenchPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "GivenNames values for the French Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "GivenNames values for the French Personnel is not present");
            }

            try
            {
                Assert.IsNotNull(IndividualTask_FrenchPersonnel);
                PropertiesCollection.test.Log(Status.Pass, "IndividualTask for the French Personnel is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IndividualTask for the French Personnel is not present");
            }

            /*******************Validation for Can Revalidate ************************************************/

            var Currency = doc.GetElementsByTagName("Currency")[0].InnerText;
            var CanRevalidate = doc.GetElementsByTagName("GetRevalidate")[0].InnerText;
            var IsRequired = doc.GetElementsByTagName("IsRequired")[0].InnerText;
            var Position = doc.GetElementsByTagName("Position")[0].HasChildNodes;
            var SlotNumber = doc.GetElementsByTagName("SlotNumber")[0].InnerText;


            try
            {
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

            try
            {
                Assert.AreEqual(CanRevalidate,"false");
                PropertiesCollection.test.Log(Status.Pass, "Can Revalidate value is 'false'");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Can Revalidate value is not 'false'");
            }

            try
            {
                Assert.AreEqual(IsRequired, "true");
                PropertiesCollection.test.Log(Status.Pass, "IsRequired value is 'true'");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsRequired value is not 'true'");
            }

            try
            {
                Assert.IsNotNull(Position);
                PropertiesCollection.test.Log(Status.Pass, "Position is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Position is not present");
            }


            try
            {
                Assert.AreEqual(SlotNumber, "1");
                PropertiesCollection.test.Log(Status.Pass, "Slot Number value is 1");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Slot Number value is not 1");
            }

        }

        [TestMethod]
        public void TC03_ReadStrip_FormedCrew_FormationGroup()
        {
            var TestCaseNo = "TC003";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_ReadStrip_FormedCrew_FormationGroup");

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["ReadStripPeople"] + testData[4], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = client.Execute(request).Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            int intRespCode = Get_StatusCode(response);
            var Slotnumber = doc.GetElementsByTagName("SlotNumber")[0].InnerText;

            try
            {        
                Assert.AreEqual(intRespCode, "200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

            try
            {  
                Assert.AreEqual(Slotnumber, "F");
                PropertiesCollection.test.Log(Status.Pass, "SlotNumber is coming as F");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "SlotNumber is not coming as F");
            }

        }

        
        [TestCategory("Regression")]
        [TestMethod]
        public void TC04_ReadPeopleStrip_MissingStripID()
        {
            String strTestCaseNo = "TC004";
            ValidateData(strTestCaseNo);
        }
        
        [TestCategory("Regression")]
        [TestMethod]
        public void TC05_ReadPeopleStrip_StripIDInvalidInteger()
        {
            String strTestCaseNo = "TC005";
            ValidateData(strTestCaseNo);
        }
        
        [TestCategory("Regression")]
        [TestMethod]
        public void TC06_ReadPeopleStrip_StripIDInvalidformat()
        {
            String strTestCaseNo = "TC006";
            ValidateData(strTestCaseNo);
        }

        //Function for validating invalid inputs -- Negative Scenarios
        public void ValidateData(String strTestCaseNo)
        {
            switch (strTestCaseNo)
            {
                case "TC004":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_ReadPeopleStrip_MissingStripID");
                    break;
                case "TC005":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC05_ReadPeopleStrip_StripIDInvalidInteger");
                    break;
                case "TC006":
                    PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC06_ReadPeopleStrip_StripIDInvalidformat");
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
                    if (strTestCaseNo == "TC004")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP100");
                    }
                    else if (strTestCaseNo == "TC005")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP101");
                    }
                    else if (strTestCaseNo == "TC006")
                    {
                        Assert.AreEqual(doc.GetElementsByTagName("Code")[0].InnerText, "FP105");
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
            else if (intRespCode == 200)
            {
                PropertiesCollection.test.Log(Status.Fail, "Validation for Test Case failed. Read Strip people could happen with incorrect input.");
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

        public IRestResponse CreateRestRequest(String TestCaseNo)
        {
            //Fetching data from My SQL Database
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            var client = new RestClient("http://oc-svr-at1/Fltpro_Automation_main/API/v1/");
            var request = new RestRequest(ConfigurationManager.AppSettings["ReadStripPeople"] + testData[4], Method.GET);

            //Adding headers to the POST request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);
       

            //Posting the Request
            IRestResponse response = client.Execute(request);

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
