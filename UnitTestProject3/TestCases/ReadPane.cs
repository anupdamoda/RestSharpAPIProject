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
    public class ReadPane
    {
        
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
        public void TC01_ReadPaneAll()
        {
            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadPaneAll");

            var client = new RestClient(ConfigurationManager.AppSettings["82-api2"]);
            var request = new RestRequest("Panes/All", Method.GET);

            //Adding headers to the request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            //Executing the Request and getting the response
            var response = client.Execute(request);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int PaneCount = doc.GetElementsByTagName("Pane").Count;

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
            
            String TableName = "tblPane";

            var conn = new ConnectToSQLServer();
            int count = conn.Count(TableName, null, null);

            try
            {               
                Assert.AreEqual(count, PaneCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Panes displayed in response is " + PaneCount + " which is as per the database");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strip Task Templates displayed in the response is not as per the database");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Pane tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Pane tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("CantCreateBriefs")[0]);
                PropertiesCollection.test.Log(Status.Pass, "CantCreateBriefs tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CantCreateBriefs tag is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("CantCreateCrewedFlights")[0]);
                PropertiesCollection.test.Log(Status.Pass, "CantCreateCrewedFlights tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CantCreateCrewedFlights tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("CantCreateStickyNotes")[0]);
                PropertiesCollection.test.Log(Status.Pass, "CantCreateStickyNotes tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CantCreateStickyNotes tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("CantCreateTasks")[0]);
                PropertiesCollection.test.Log(Status.Pass, "CantCreateTasks tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "CantCreateTasks tag is not present");
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
                Assert.IsNotNull(doc.GetElementsByTagName("ImpoundPane")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ImpoundPane tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ImpoundPane tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("IsSynthetic")[0]);
                PropertiesCollection.test.Log(Status.Pass, "IsSynthetic tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "IsSynthetic tag is not present");
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
                Assert.IsNotNull(doc.GetElementsByTagName("Name")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Name tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Name tag is not present");
            }            
            if (doc.GetElementsByTagName("Pane")[0].SelectSingleNode("PaneRowDefinitions") != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("//PaneRowDefinition[contains(.,'AssetRegistration')]"));
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetRegistration tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetRegistration tag is not present");
                }                
                if (doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0] != null)
                {
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0]);
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType tag is not present");
                    }
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0].SelectNodes("Code")[0]);
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.Code tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.Code tag is not present");
                    }
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0].SelectNodes("ID")[0]);
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.ID tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.ID tag is not present");
                    }
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0].SelectNodes("//AssetType[contains(.,'LastUpdated')]"));
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.LastUpdated tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.LastUpdated tag is not present");
                    }
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0].SelectNodes("LastUpdatedBy")[0]);
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.LastUpdatedBy tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.LastUpdatedBy tag is not present");
                    }
                    try
                    {
                        Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("AssetType")[0].SelectNodes("Name")[0]);
                        PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.Name tag is present");
                    }
                    catch
                    {
                        PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.AssetType.Name tag is not present");
                    }
                }

                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("ID")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.ID tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.ID tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("LastUpdated")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.LastUpdated tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.LastUpdated tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("LastUpdatedBy")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.LastUpdatedBy tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.LastUpdatedBy tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("//PaneRowDefinition[contains(.,'RowColour')]"));
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.RowColour tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.RowColour tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("RowNumber")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.RowNumber tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition tag.RowNumber is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PaneRowDefinitions")[0].SelectNodes("PaneRowDefinition")[0].SelectNodes("//PaneRowDefinition[contains(.,'RowTitle')]"));
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PaneRowDefinitions.PaneRowDefinition.RowTitle tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PaneRowDefinitions.PaneRowDefinition.RowTitle tag is not present");
                }
            }

            if (doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("ID")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.ID tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.ID tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("IsActive")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.IsActive tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.IsActive tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("LastUpdated")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.LastUpdated tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.LastUpdated tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("LastUpdatedBy")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.LastUpdatedBy tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.LastUpdatedBy tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("LocationCode")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.LocationCode tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.LocationCode tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("Name")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.Name tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.Name tag is not present");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Pane")[0].SelectNodes("PeopleGroup")[0].SelectNodes("Type")[0]);
                    PropertiesCollection.test.Log(Status.Pass, "Pane.PeopleGroup.Type tag is present");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Pane.PeopleGroup.Type tag is not present");
                }
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("PrimaryPane")[0]);
                PropertiesCollection.test.Log(Status.Pass, "PrimaryPane tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "PrimaryPane tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("ProductionPane")[0]);
                PropertiesCollection.test.Log(Status.Pass, "ProductionPane tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "ProductionPane tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("SiteID")[0]);
                PropertiesCollection.test.Log(Status.Pass, "SiteID tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "SiteID tag is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("SweeperLine")[0]);
                PropertiesCollection.test.Log(Status.Pass, "SweeperLine tag is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "SweeperLine tag is not present");
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
