using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject3.Database
{
    class ConnectToMySQL_Update_TestData
    {

        private MySqlConnection connection;

        //Constructor
        public ConnectToMySQL_Update_TestData()
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

        public string[] Update(string strtblname, string strColumnName, string strColumnValue, string strTestCaseNumber)
        {

            string query = "Update " + strtblname + " set " + strColumnName + " = '" + strColumnValue + "' where TestCaseNo = '" + strTestCaseNumber + "'";

            //Create a list to store the result
            String[] TestData = new string[12];

            //Open connection
            if (this.OpenConnection() == true)
            {

                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();


                //close Data Reader
                //  dataReader.Close();

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
