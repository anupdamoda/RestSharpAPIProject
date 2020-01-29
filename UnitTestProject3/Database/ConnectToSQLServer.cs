using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject3.Database
{
    class ConnectToSQLServer
    {
        public SqlConnection sqlCon;

        /* Constructor */
        public ConnectToSQLServer()
        {
            Initialize();
        }

        /* Connect to SQL Server */
        private void Initialize()
        {

            string connectionString = "Data Source=" + ConfigurationManager.AppSettings["SQLServerDataSource"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["SQLServerInitialCatalog"] + ";Integrated Security=" + ConfigurationManager.AppSettings["SQLServerIntegratedSecurity"] + ';';
            sqlCon = new SqlConnection(connectionString);
        }


        /* Open Database conenction */
        private bool OpenConnection()
        {
            try
            {
                sqlCon.Open();
                return true;
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        /* Close Database conenction */
        private bool CloseConnection()
        {
            try
            {
                sqlCon.Close();
                return true;
            }
            catch (SqlException excep)
            {
                Console.WriteLine(excep.Message);
                return false;
            }

        }

        
        /* Delete statement */
        public bool Delete(string strtblname, string strtblcolumn, string strwherecondn)
        {
            string str = "DELETE from " + strtblname + " WHERE " + strtblcolumn + "='" + strwherecondn + "';";
            Console.WriteLine(str);
            if (this.OpenConnection() == true)
            {
                SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                sqlcmd.ExecuteNonQuery();

                try
                {
                    this.CloseConnection();
                    return true;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return false;
                }
            }
            else
            {
                this.CloseConnection();
                return false;
            }
        }

        /* Select statement to select a specific value */
        public string Select(string tblname, string tblselectcolumn, string tblcolumn, string wherecondn)
        {
            string str = "SELECT " + tblselectcolumn + " from " + tblname + " WHERE " + tblcolumn + "='" + wherecondn + "';";
            string result = null;

            Console.WriteLine(str);
            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                    result = sqlcmd.ExecuteScalar().ToString();
                    Console.WriteLine(result);
                    this.CloseConnection();
                    return result;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return result;
                }
            }
            else
            {
                this.CloseConnection();
                return result;
            }
        }


        /* Get count  */
        public int Count(string tblname, string tblcolumn, string wherecondn)
        {
            String str = null;
            if (tblcolumn == null && wherecondn == null)
            {
                str = "SELECT count(*) FROM " + tblname  + ';';
            }
            else
            {
                str = "SELECT count(*) FROM " + tblname + " WHERE " + tblcolumn + "='" + wherecondn + "';";
            }
            
            int result = 0;
            
            Console.WriteLine(str);

            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                    result = (int)sqlcmd.ExecuteScalar();
                    this.CloseConnection();
                    return result;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return result;
                }
            }
            else
            {
                this.CloseConnection();
                return result;
            }
        }


        /* Get count  */
        public int StripTaskTemplateCountforAssetTypeID(string AssetTypeID)
        {
            
            String str = "SELECT count(*) FROM tblDefaultTaskTextExtra a , tblDefaultTaskTextPane b where a.TypeKeyBigID =" + AssetTypeID + "AND a.ExtraTypeID = 0 AND a.TypeKeyID =0 AND a.DefaultTaskTextID = b.DefaultTaskTextID";
       
            int result = 0;

            Console.WriteLine(str);

            if (this.OpenConnection() == true)
            {
                try
                {
                    SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                    result = (int)sqlcmd.ExecuteScalar();
                    this.CloseConnection();
                    return result;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return result;
                }
            }
            else
            {
                this.CloseConnection();
                return result;
            }
        }


        /* Assign Planning Board To Security Group */
        public bool AssignPlanningBoardToSecurityGroup(string strPlanningBoardName, string strSecurityGroupName)
        {
            string str = "DECLARE @boardName nvarchar(50) = '" + strPlanningBoardName + "'; DECLARE @securityGroupName nvarchar(50) = '" + strSecurityGroupName + "';  DECLARE @newID bigint; DECLARE @base bigint = (SELECT StaticConfigValInt FROM tblStaticConfig WHERE StaticConfigID = 50) *10000000000; DECLARE @boardID bigint = 0; SELECT @boardID = (SELECT TOP(1) PlanningBoardID FROM tblPlanningBoard WHERE  Name = @boardName); DECLARE @securityGroupID bigint = 0; SELECT @securityGroupID = (SELECT TOP(1) SecurityGroupID FROM tblSecurityGroup WHERE SecurityGroupName = @securityGroupName); IF @boardID > 0 AND @securityGroupID > 0 BEGIN IF NOT EXISTS (SELECT 1 FROM tblSecurityGroupObjectData WHERE SecurityGroupID = @securityGroupID AND LinkID = @boardID AND SecurityObjectID = 220) BEGIN EXEC FPNextRecordID  'tblSecurityGroupObjectData', 'SecurityGroupObjectDataID', 1, 1, @newID OUT PRINT 'NewID: ' + CAST(@newID AS nvarchar); INSERT INTO tblSecurityGroupObjectData (SecurityGroupObjectDataID, SecurityGroupID, SecurityObjectID, SecurityLevel, LinkID, LastUpdated, LoginName) VALUES (@base + @newID, @securityGroupID, 220, 4, @boardID, GETUTCDATE(), 'Automation') END ELSE PRINT 'Security already exists for ' + @boardName + ' (' + CAST(@boardID as nvarchar) + ') and ' + @securityGroupName + ' (' +  CAST(@securityGroupID as nvarchar) + ').' END ELSE PRINT 'Planning Board and/or Security Group not found.' ";

            if (this.OpenConnection() == true)
            {
                SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                sqlcmd.ExecuteNonQuery();

                try
                {
                    this.CloseConnection();
                    return true;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return false;
                }
            }
            else
            {
                this.CloseConnection();
                return false;
            }

        }

        /* Run SQL */
        public bool RunSQL(string str)
        {
            if (this.OpenConnection() == true)
            {
                SqlCommand sqlcmd = new SqlCommand(str, sqlCon);
                sqlcmd.ExecuteNonQuery();

                try
                {
                    this.CloseConnection();
                    return true;
                }
                catch (SqlException excep)
                {
                    Console.WriteLine(excep.Message);
                    return false;
                }
            }
            else
            {
                this.CloseConnection();
                return false;
            }
        }
    }
}





