using Microsoft.Data.SqlClient;
using System.Data;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace HomeDatabase
{
    public class SqlConnect
    {

        public static string conStr;

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(conStr);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }







    }
}
