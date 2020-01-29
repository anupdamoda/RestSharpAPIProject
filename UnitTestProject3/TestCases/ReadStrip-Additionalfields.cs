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
    public class ReadStripAdditionalfields
    {

        String strtblname = "automation_strips";
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
        public void TC01_ReadStrip_Additionalfields()
        {
            var TestCaseNo = "TC01"; 
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testData = connection.Select(strtblname, TestCaseNo, strTestType);

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_ReadStrip_Additionalfields");
            

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("Strips/" + testData[21], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

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
                Assert.IsNotNull(doc.GetElementsByTagName("Documents")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Documents  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Documents  - Additional field is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("FreightOn")[0]);
                PropertiesCollection.test.Log(Status.Pass, "FreightOn  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "FreightOn  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("FreightOff")[0]);
                PropertiesCollection.test.Log(Status.Pass, "FreightOff  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "FreightOff  - Additional field is not present");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("FuelOn")[0]);
                PropertiesCollection.test.Log(Status.Pass, "FuelOn  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "FuelOn  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("FuelOff")[0]);
                PropertiesCollection.test.Log(Status.Pass, "FuelOff  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "FuelOff  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("PaxOn")[0]);
                PropertiesCollection.test.Log(Status.Pass, "PaxOn  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "PaxOn  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("PaxOff")[0]);
                PropertiesCollection.test.Log(Status.Pass, "PaxOff  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "PaxOff  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Slot")[0]);
                PropertiesCollection.test.Log(Status.Pass, "Slot  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Slot  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("TaskIdentifier")[0]);
                PropertiesCollection.test.Log(Status.Pass, "TaskIdentifier  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "TaskIdentifier  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UserStatus1")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UserStatus1  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UserStatus1  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("UserStatus2")[0]);
                PropertiesCollection.test.Log(Status.Pass, "UserStatus2  - Additional field is present");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "UserStatus2  - Additional field is not present");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Asset")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Asset entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Asset entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Asset")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Asset entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Asset entity");
            }

            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("Asset")[0].SelectNodes("Group")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Asset")[0].SelectNodes("Group")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Asset.Group entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Asset.Group entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Asset")[0].SelectNodes("Group")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Asset.Group entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Asset.Group entity");
                }
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("AssetType")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Asset entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.AssetType entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("AssetType")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.AssetType entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.AssetType entity");
            }

            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("Authorizations")[0].SelectNodes("StripAuthorizations")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Authorizations")[0].SelectNodes("StripAuthorizations")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Authorizations.StripAuthorizations entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Authorizations.StripAuthorizations entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Authorizations")[0].SelectNodes("StripAuthorizations")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Authorizations.StripAuthorizations entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Authorizations.StripAuthorizations entity");
                }
            }
           
            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("CustomFields")[0].SelectNodes("CustomField")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("CustomFields")[0].SelectNodes("CustomField")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.CustomFields.CustomField entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.CustomFields.CustomField entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("CustomFields")[0].SelectNodes("CustomField")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.CustomFields.CustomField entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.CustomFields.CustomField entity");
                }
            } 

            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("Documents")[0].SelectNodes("StripDocument")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Documents")[0].SelectNodes("StripDocument")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Documents.StripDocument entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Documents.StripDocument entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Documents")[0].SelectNodes("StripDocument")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Documents.StripDocument entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Documents.StripDocument entity");
                }
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("ExternalSystem")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.ExternalSystem entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.ExternalSystem entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("ExternalSystem")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.ExternalSystem. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.ExternalSystem entity");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("HoursCode")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.HoursCode entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.HoursCode entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("HoursCode")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.HoursCode. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.HoursCode entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LocationTo")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.LocationTo entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.LocationTo entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LocationTo")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.LocationTo. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.LocationTo entity");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LocationFrom")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.LocationFrom entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.LocationFrom entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("LocationFrom")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.LocationFrom. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.LocationFrom entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("MaximumWeatherState")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.MaximumWeatherState entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.MaximumWeatherState entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("MaximumWeatherState")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.MaximumWeatherState. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.MaximumWeatherState entity");
            }

            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("MinimumWeatherState")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.MinimumWeatherState entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.MinimumWeatherState entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("MinimumWeatherState")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.MinimumWeatherState. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.MinimumWeatherState entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Pane")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Pane entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Pane entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Pane")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Pane. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Pane entity");
            }
            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("Currencies")[0].SelectNodes("Currency")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("Currencies")[0].SelectNodes("Currency")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Slots.Slot.Currencies.Currency entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Slots.Slot.Currencies.Currency entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("Currencies")[0].SelectNodes("Currency")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Slots.Slot.Currencies.Currency entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Slots.Slot.Currencies.Currency entity");
                }
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("Position")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Slots.Slot.Position entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Slots.Slot.Position entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("Position")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Slots.Slot.Position entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Slots.Slot.Position entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("StripPerson")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Slots.Slot.StripPerson entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Slots.Slot.StripPerson entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("StripPerson")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Slots.Slot.StripPerson entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Slots.Slot.StripPerson entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("StripPerson")[0].SelectNodes("Person")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.Slots.Slot.StripPerson.Person entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.Slots.Slot.StripPerson.Person entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("Slots")[0].SelectNodes("Slot")[0].SelectNodes("StripPerson")[0].SelectNodes("Person")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.Slots.Slot.StripPerson.Person entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.Slots.Slot.StripPerson.Person entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("SubGroup")[0].SelectNodes("LastUpdated"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.SubGroup entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.SubGroup entity");
            }
            try
            {
                Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("SubGroup")[0].SelectNodes("LastUpdatedBy"));
                PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.SubGroup. entity");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.SubGroup entity");
            }
            if (doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0] != null)
            {
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.TimeAllocations.TimeAllocation entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.TimeAllocations.TimeAllocation entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.TimeAllocations.TimeAllocation entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.TimeAllocations.TimeAllocation entity");
                }
         
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("HoursCode")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.TimeAllocations.TimeAllocation.HoursCode entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.TimeAllocations.TimeAllocation.HoursCode entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("HoursCode")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.TimeAllocations.TimeAllocation.HoursCode entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.TimeAllocations.TimeAllocation.HoursCode entity");
                }

                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.TimeAllocations.TimeAllocation.Person entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.TimeAllocations.TimeAllocation.Person entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.TimeAllocations.TimeAllocation.Person entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.TimeAllocations.TimeAllocation.Person entity");
                }

                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("PeopleGroup")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.TimeAllocations.TimeAllocation.Person.PeopleGroup entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.TimeAllocations.TimeAllocation.Person.PeopleGroup entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("PeopleGroup")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.TimeAllocations.TimeAllocation.Person.PeopleGroup entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.TimeAllocations.TimeAllocation.Person.PeopleGroup entity");
                }

                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("OrganisationGroup")[0].SelectNodes("LastUpdated"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated present for Strip.TimeAllocations.TimeAllocation.Person.OrganisationGroup entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated not present for Strip.TimeAllocations.TimeAllocation.Person.OrganisationGroup entity");
                }
                try
                {
                    Assert.IsNotNull(doc.GetElementsByTagName("Strip")[0].SelectNodes("TimeAllocations")[0].SelectNodes("TimeAllocation")[0].SelectNodes("Person")[0].SelectNodes("OrganisationGroup")[0].SelectNodes("LastUpdatedBy"));
                    PropertiesCollection.test.Log(Status.Pass, "Last Updated By present for Strip.TimeAllocations.TimeAllocation.Person.OrganisationGroup entity");
                }
                catch
                {
                    PropertiesCollection.test.Log(Status.Fail, "Last Updated By not present for Strip.TimeAllocations.TimeAllocation.Person.OrganisationGroup entity");
                }
            }
        }

        [TestMethod]
        public void TC02_ReadStrip_Additionalfields_InvalidStripID()
        {
            var TestCaseNo = "TC02";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testdata_details = connection.Select(strtblname, TestCaseNo, strTestType);

            string strTDCode = testdata_details[22];
            string strTDMessage = testdata_details[23];

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC02_ReadStrip_Additionalfields_InvalidStripID");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("Strips/" + testdata_details[21], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripCount = doc.GetElementsByTagName("Strip").Count;
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText;

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Assert.AreEqual(intRespCode, 404);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(0, StripCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strips is " + StripCount);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strips is " + StripCount);
            }

            try
            {
                Assert.AreEqual(Code, strTDCode);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, strTDMessage);
                PropertiesCollection.test.Log(Status.Pass, "Error Message '" + Message + "' is valid" );
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Message is invalid");
            }
        }

            [TestMethod]
        public void TC03_ReadStrip_Additionalfields_InvalidDataFormatStripID()
        {
            var TestCaseNo = "TC03";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testdata_details = connection.Select(strtblname, TestCaseNo, strTestType);

            string strTDCode = testdata_details[22];
            string strTDMessage = testdata_details[23];

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC03_ReadStrip_Additionalfields_InvalidDataFormatStripID");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("Strips/" + testdata_details[21], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripCount = doc.GetElementsByTagName("Strip").Count;
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText.Substring(0,31);
            Console.WriteLine(Message);

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Console.WriteLine("intResponse: " + intRespCode);
                Assert.AreEqual(intRespCode, 400);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(0, StripCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strips is "+ StripCount);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strips is " + StripCount);
            }

            try
            {
                Assert.AreEqual(Code, strTDCode);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, strTDMessage);
                PropertiesCollection.test.Log(Status.Pass, "Error Message '" + Message + "' is valid");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Message is invalid");
            }
        }

        [TestMethod]
        public void TC04_ReadStrip_Additionalfields_MissingStripID()
        {
            var TestCaseNo = "TC04";
            var connection = new ConnectToMySQL_Fetch_TestData();
            var testdata_details = connection.Select(strtblname, TestCaseNo, strTestType);

            string strTDCode = testdata_details[22];
            string strTDMessage = testdata_details[23];

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC04_ReadStrip_Additionalfields_MissingStripID");


            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest("Strips/" + testdata_details[21], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);
            int StripCount = doc.GetElementsByTagName("Strip").Count;
            var Code = doc.GetElementsByTagName("Code")[0].InnerText;
            var Message = doc.GetElementsByTagName("Message")[0].InnerText;
            Console.WriteLine(Message);

            //Retrieving & printing the Response code

            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            try
            {
                Console.WriteLine("intResponse: " + intRespCode);
                Assert.AreEqual(intRespCode, 405);
                PropertiesCollection.test.Log(Status.Pass, "Status Response is " + intRespCode);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is " + intRespCode);
            }

            try
            {
                Assert.AreEqual(0, StripCount);
                PropertiesCollection.test.Log(Status.Pass, "Count of Strips is " + StripCount);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Count of Strips is " + StripCount);
            }

            try
            {
                Assert.AreEqual(Code, strTDCode);
                PropertiesCollection.test.Log(Status.Pass, "Error Code is " + Code);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Code is " + Code);
            }

            try
            {
                Assert.AreEqual(Message, strTDMessage);
                PropertiesCollection.test.Log(Status.Pass, "Error Message '" + Message + "' is valid");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Error Message is invalid");
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
