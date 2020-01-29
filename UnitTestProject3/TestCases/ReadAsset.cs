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
    public class ReadAsset
    {



        public int Get_StatusCode(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int intRespCode = (int)statusCode;

            return intRespCode;
        }

        int intAssetCount;

       
        [TestCategory("Regression")]
        [TestMethod]
        public void TC01_GetAsset()
        {

            PropertiesCollection.test = PropertiesCollection.extent.CreateTest("TC01_GetAsset");
            Object[] ObjDBResponse = new object[16];

            //Connecting to application database and retrieving the current count of documents attached to the strip
            string strConnectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();

            SqlDataReader reader = null;

            String strQuery = "select count(*) from dbo.tblAsset";

            SqlCommand command = new SqlCommand(strQuery, myConnection);
            reader = command.ExecuteReader();


            while (reader.Read())
            {
                intAssetCount = reader.GetInt32(0);
            }
            reader.Close();

            intAssetCount = intAssetCount - 1;

            var client = new RestClient(ConfigurationManager.AppSettings["BaseURL"]);
            var request = new RestRequest(ConfigurationManager.AppSettings["ReadAsset"], Method.GET);

            //Adding headers to the GET request
            request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"]);
            request.AddHeader("X-ExternalRequest-ID", ConfigurationManager.AppSettings["X-ExternalRequest-ID"]);
            request.AddHeader("X-ExternalSystem-ID", ConfigurationManager.AppSettings["X-ExternalSystem-ID"]);
            request.AddHeader("X-Date", ConfigurationManager.AppSettings["X-Date"]);

            IRestResponse response = client.Execute(request);
            var content = client.Execute(request).Content;

            Console.WriteLine("Content Length: " + content.Length);
            Console.WriteLine("Response Status: " + response.ResponseStatus);

            Console.WriteLine("Status Code: " + response.StatusCode);
            Console.WriteLine("Header Count: " + response.Headers.Count);

            var doc = new XmlDocument();
            doc.LoadXml(response.Content);

            int AssetCountAPI = doc.GetElementsByTagName("Asset").Count;

            Console.WriteLine("Count of Assets from Read API: " + AssetCountAPI);

            int intRespCode = Get_StatusCode(response);

            Console.WriteLine(doc.GetElementsByTagName("Group")[0].ParentNode);

            Boolean LastUpdated = doc.GetElementsByTagName("LastUpdated")[0].HasChildNodes;
            Boolean LastUpdatedby = doc.GetElementsByTagName("LastUpdatedBy")[0].HasChildNodes;

            Boolean Group = doc.GetElementsByTagName("Group")[0].HasChildNodes;
            Boolean AssetSystem = doc.GetElementsByTagName("AssetSystem")[0].HasChildNodes;
            Boolean Unavailabilities = doc.GetElementsByTagName("Unavailabilities")[0].HasChildNodes;

            var AssetChildNodes = doc.GetElementsByTagName("Asset")[0].ChildNodes;
            Console.WriteLine("Child Nodes under Asset: " + AssetChildNodes);

            var ID = doc.GetElementsByTagName("ID")[0].InnerText;
            Console.WriteLine("ID: " + ID);

          
            try
            {
                Console.WriteLine(intRespCode);
                Assert.AreEqual(intRespCode,"200");
                PropertiesCollection.test.Log(Status.Pass, "Status Response is 200 OK");
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Status Response is not 200 OK");
            }

            try
            {
                Assert.AreEqual(intAssetCount,AssetCountAPI);
                PropertiesCollection.test.Log(Status.Pass,"Count of Assets returned in the API: " + AssetCountAPI + "matches the Count of Assets present in the FlightPro Applicaiton Database: " +  intAssetCount);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Pass, "Count of Assets returned in the API: " + AssetCountAPI + "doesnt match the Count of Assets present in the FlightPro Applicaiton Database: " + intAssetCount);
            }

            try
            {
                Assert.IsTrue(LastUpdated);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdated is present: " + LastUpdated);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdated is not present: " + LastUpdated);
            }

            try
            {
                Assert.IsTrue(LastUpdatedby);
                PropertiesCollection.test.Log(Status.Pass, "LastUpdatedBy is present: " + LastUpdatedby);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "LastUpdatedBy is not present: " + LastUpdatedby);
            }

            try
            {
                Assert.IsTrue(Group);
                PropertiesCollection.test.Log(Status.Pass, "Group is present: " + Group);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Group is not present: " + Group);
            }


            try
            {
                Assert.IsTrue(Unavailabilities);
                PropertiesCollection.test.Log(Status.Pass, "Unavailabilities is present: " + Unavailabilities);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "Unavailabilities is not present: " + Unavailabilities);
            }

            try
            {
                Assert.IsTrue(AssetSystem);
                PropertiesCollection.test.Log(Status.Pass, "AssetSystem is present: " + AssetSystem);
            }
            catch
            {
                PropertiesCollection.test.Log(Status.Fail, "AssetSystem is not present: " + AssetSystem);
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


        [ClassCleanup]
        public static void ClassCleanup()
        {
            PropertiesCollection.extent.Flush();
        }


    }


    }
 

