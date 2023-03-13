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






    }
}
