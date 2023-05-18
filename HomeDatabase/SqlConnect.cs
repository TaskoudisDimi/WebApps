using HomeDatabase.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace HomeDatabase
{
    public class SqlConnect
    {

        public static string conStr;
        private static SqlConnection con = new SqlConnection();
        
        public DataTable table = new DataTable();

        public SqlConnect()
        {
            if(con.State != ConnectionState.Open)
                con.ConnectionString = conStr;
            
        }

        public void OpenCon()
        {
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CloseCon()
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void retrieveData(string command)
        {
            try
            {
                
                SqlDataAdapter adapter = new SqlDataAdapter(command, con);
                adapter.Fill(table);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }


        //Get Databases
        public List<Databases> GetDatabaseList()
        {
            List<Databases> listDB = new List<Databases>();
            // Open connection to the database
            string conString = "server=DIMITRISTASKOUD\\DIMITRIS_TASKOUD;database=Home;Integrated Security=SSPI;TrustServerCertificate=True;";
            //string conString = "server=192.168.24.177,51434;database=Home;Integrated Security=SSPI;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Databases databases = new Databases();
                            databases.Name = reader["Name"].ToString();
                            if(!databases.Name.Contains("master") && !databases.Name.Contains("tempdb") && !databases.Name.Contains("model") &&
                                !databases.Name.Contains("msdb"))
                            listDB.Add(databases);
                        }
                    }
                }
            }
            return listDB;
        }

        public void execCom(string cmd)
        {
            OpenCon();
            try
            {
                SqlCommand command = new SqlCommand(cmd, con);
                command.ExecuteNonQuery();
            }
            catch
            {

            }
        }


    }
}
