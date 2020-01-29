using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace UnitTestProject3.Database
{
    class ConnectToMySQL_Fetch_TestData
    {
        private MySqlConnection connection;

        //Constructor
        public ConnectToMySQL_Fetch_TestData()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString = "server=" + ConfigurationManager.AppSettings["TestDataDBServer"] + ";user=" + ConfigurationManager.AppSettings["TestDataDBUsername"] + ";database=" + ConfigurationManager.AppSettings["TestDataDBDatabase"] + ";password=" + ConfigurationManager.AppSettings["TestDataDBPassword"];
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public string[] Select(string strtblname, string strTestCaseNo, string strTestType)
        {

            string query = "SELECT * FROM " + strtblname + " where TestCaseNo='" + strTestCaseNo + "' and TestType='" + strTestType + "'";

            //Create a list to store the result
            String[] TestData = new string[30];

            //Open connection
            if (this.OpenConnection() == true)
            {

                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                switch (strtblname)

                {
                    case "automation_stripdocument":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("StripDocumentID"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("StripID"));
                            TestData[5] = dataReader.GetString(dataReader.GetOrdinal("Notes"));
                            TestData[6] = dataReader.GetString(dataReader.GetOrdinal("DocumentData"));
                            TestData[7] = dataReader.GetString(dataReader.GetOrdinal("Title"));
                            TestData[8] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                            TestData[9] = dataReader.GetString(dataReader.GetOrdinal("FileName"));
                        }
                        break;


                    case "automation_waypoint":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("Colour"));
                            TestData[5] = dataReader.GetString(dataReader.GetOrdinal("DisplayCode"));
                            TestData[6] = dataReader.GetString(dataReader.GetOrdinal("Latitude"));
                            TestData[7] = dataReader.GetString(dataReader.GetOrdinal("Longitude"));
                            TestData[8] = dataReader.GetString(dataReader.GetOrdinal("LocationName"));
                            TestData[9] = dataReader.GetString(dataReader.GetOrdinal("ShortCode"));
                            TestData[10] = dataReader.GetString(dataReader.GetOrdinal("TimeZone"));
                            TestData[11] = dataReader.GetString(dataReader.GetOrdinal("UseDayLightSavings"));
                            TestData[12] = dataReader.GetString(dataReader.GetOrdinal("WaypointID"));
                        }
                        break;

                    case "automation_StripPeople":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("StripId"));
                        }
                        break;

                    


                    case "automation_createunavailability":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("AssetID"));
                            TestData[5] = dataReader.IsDBNull(dataReader.GetOrdinal("SubTypeID")) ? "" : dataReader.GetString(dataReader.GetOrdinal("SubTypeID"));
                            TestData[6] = dataReader.GetString(dataReader.GetOrdinal("Details"));
                            TestData[7] = dataReader.GetString(dataReader.GetOrdinal("Details2"));
                            TestData[8] = dataReader.GetString(dataReader.GetOrdinal("StripType"));
                            TestData[9] = dataReader.IsDBNull(dataReader.GetOrdinal("PeopleID")) ? "" : dataReader.GetString(dataReader.GetOrdinal("PeopleID"));
                        }
                        break;


                    case "automation_striptasktemplate":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("PaneID"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("AssetTypeID"));
                            TestData[5] = dataReader.GetString(dataReader.GetOrdinal("Result"));
                            TestData[6] = dataReader.GetString(dataReader.GetOrdinal("Code"));
                            TestData[7] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                        }
                        break;

                    case "automation_strips":
                        while (dataReader.Read())
                        {
                            TestData[0] = dataReader.GetString(dataReader.GetOrdinal("TestCaseNo"));
                            TestData[1] = dataReader.GetString(dataReader.GetOrdinal("TestCaseName"));
                            TestData[2] = dataReader.GetString(dataReader.GetOrdinal("Environment"));
                            TestData[3] = dataReader.GetString(dataReader.GetOrdinal("TestType"));
                            TestData[4] = dataReader.GetString(dataReader.GetOrdinal("AssetRegistration"));
                            TestData[5] = dataReader.GetString(dataReader.GetOrdinal("AssetTypeID"));
                            TestData[6] = dataReader.GetString(dataReader.GetOrdinal("CallSign"));
                            TestData[7] = dataReader.GetString(dataReader.GetOrdinal("LocationFromID"));
                            TestData[8] = dataReader.GetString(dataReader.GetOrdinal("LocationToID"));
                            TestData[9] = dataReader.GetString(dataReader.GetOrdinal("PlannedStartTime"));
                            TestData[10] = dataReader.GetString(dataReader.GetOrdinal("PlannedEndTime"));
                            TestData[11] = dataReader.GetString(dataReader.GetOrdinal("PaneId"));
                            TestData[12] = dataReader.GetString(dataReader.GetOrdinal("PersonID1"));
                            TestData[13] = dataReader.GetString(dataReader.GetOrdinal("SlotNumber1"));
                            TestData[14] = dataReader.GetString(dataReader.GetOrdinal("StripTask"));
                            TestData[15] = dataReader.GetString(dataReader.GetOrdinal("StripType"));
                            TestData[16] = dataReader.GetString(dataReader.GetOrdinal("WeatherStateID"));
                            TestData[17] = dataReader.GetString(dataReader.GetOrdinal("PersonID2"));
                            TestData[18] = dataReader.GetString(dataReader.GetOrdinal("SlotNumber2"));
                            TestData[19] = dataReader.GetString(dataReader.GetOrdinal("GroupHeaderAssetTypeID"));
                            TestData[20] = dataReader.GetString(dataReader.GetOrdinal("UnavailabilitySubTypeID"));
                            TestData[21] = dataReader.GetString(dataReader.GetOrdinal("StripID"));
                            TestData[22] = dataReader.GetString(dataReader.GetOrdinal("Code"));
                            TestData[23] = dataReader.GetString(dataReader.GetOrdinal("Message"));
                        }

                        break;

                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return TestData;
            }
            else
            {
                return TestData;
            }
        }
    }
}
