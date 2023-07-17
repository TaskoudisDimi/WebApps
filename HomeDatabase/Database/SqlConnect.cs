using HomeDatabase.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace HomeDatabase.Database
{
    public class SqlConnect
    {

        public static int queryTimeOut = 20;
        static Dictionary<int, SqlConnect> instances = new Dictionary<int, SqlConnect>();
        private static int instanceID;
        private static readonly ThreadLocal<int> currentInstanceId = new ThreadLocal<int>();
        public static string connectionString;
        public SqlDataReader reader = null;
        public SqlConnection connection = null;

        public static SqlConnect Instance
        {
            get
            {
                instanceID = currentInstanceId.Value;
                SqlConnect res = new SqlConnect();
                using (Locker.Lock(instances))
                {
                    if (!instances.ContainsKey(instanceID))
                    {
                        instances[instanceID] = res;
                        return res;
                    }
                    else
                    {
                        return instances[instanceID];
                    }
                }
            }
        }

        //if true = Create Connection
        public bool CheckConnection()
        {
            try
            {
                CloseConnection();
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, "CheckConnection");
            }
            if (connection == null || connection.State != ConnectionState.Open)
            {
                return openConnection();
            }
            return true;
        }

        private void CloseConnection(bool OK = true)
        {
            try
            {
                if (connection == null)
                    return;
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, "CheckConnection");
            }

        }

        public bool openConnection()
        {
            CloseConnection();
            connection = new SqlConnection(connectionString);
            try
            {
                using (Locker.Lock(instances))
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, "OpenConnection");
                connection.Dispose();
                connection = null;
                instances.Remove(currentInstanceId.Value);
                SqlConnection.ClearAllPools();
            }
            return connection != null;
        }

        public SqlDataReader SelectDataReader(string sql, List<SqlParameter> parameters = null)
        {
            
            if (!CheckConnection())
            {
                return null;
            }
            using (Locker.Lock(instances))
            {
                DateTime dtStart = DateTime.Now;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = queryTimeOut;
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    reader = cmd.ExecuteReader();
                    instances.Remove(currentInstanceId.Value);
                }
                catch
                {

                }
                long dt = (long)((DateTime.Now - dtStart).TotalMilliseconds);
                if (dt > 100)
                {
                    Helper.Log("Sql query is : " + sql + "And the time is : " + dt, "Query");
                }
            }
            return reader;
        }

        public int Insert(string cmd)
        {
            if (!CheckConnection())
            {
                return -1;
            }
            try
            {
                SqlCommand command = new SqlCommand(cmd, connection);
                return command.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }

        }

        public int Update(string cmd)
        {
            if (!CheckConnection())
            {
                return -1;
            }
            try
            {
                SqlCommand command = new SqlCommand(cmd, connection);
                return command.ExecuteNonQuery();

            }
            catch
            {
                return -1;
            }
        }

        public int Delete(string cmd)
        {
            if (!CheckConnection())
            {
                return -1;
            }
            try
            {
                SqlCommand command = new SqlCommand(cmd, connection);
                return command.ExecuteNonQuery();

            }
            catch
            {
                return -1;
            }
        }

        public DataTable SelectDataTable(string sql, List<SqlParameter> parameters = null)
        {
            DataTable dt = new DataTable();
            DateTime dtStart = DateTime.Now;
            if (!CheckConnection())
            {
                return null;
            }
            using (Locker.Lock(instances))
            {
                try
                {
                    reader = SelectDataReader(sql, parameters);
                    dt.Load(reader); 
                }
                catch
                {

                }
                long time = (long)((DateTime.Now - dtStart).TotalMilliseconds);
                if (time > 100)
                {
                    Helper.Log("Sql query is : " + sql + "And the time is : " + time, "Query");
                }
            }
            return dt;
        }

        public DataSet ExecDataSet()
        {
            DataSet set = new DataSet();

            return set;
        }

        //Get Databases
        public List<DatabasesModel> GetDatabaseList()
        {
            List<DatabasesModel> listDB = new List<DatabasesModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT database_id, name from sys.databases", con))
                {
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatabasesModel databases = new DatabasesModel();
                            databases.Name = reader["Name"].ToString();
                            databases.id = Convert.ToInt32(reader["database_id"]);
                            if (!databases.Name.Contains("master") && !databases.Name.Contains("tempdb") && !databases.Name.Contains("model") &&
                                !databases.Name.Contains("msdb"))
                                listDB.Add(databases);
                        }
                    }
                }
            }
            return listDB;
        }

        //Get Tables
        public List<TableViewModel> GetTables()
        {
            List<TableViewModel> listServers = new List<TableViewModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("use HomeDB\r\nSELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ", con))
                {
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableViewModel databases = new TableViewModel();
                            databases.TableName = reader["TABLE_NAME"].ToString();
                            listServers.Add(databases);
                        }
                    }
                }
            }

            return listServers;
        }

        public void RemoveAllInstances()
        {
            try
            {
                instances.Clear();
            }
            catch
            {

            }

        }

        public void DisposeTable(DataTable table)
        {
            try
            {
                table.Dispose();
            }
            catch
            {

            }
        }


        //Execute StoreProcedure
        public bool CleanDB()
        {
            bool result = false;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_CleanDatabaseTables", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        connection.Close();
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }


        public void RestoreDB(string pathDB, string NameOfDatabase)
        {
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand useMaster = new SqlCommand("USE master", connection))
                    {
                        useMaster.ExecuteNonQuery();
                    }
                    string restoreDB = $"RESTORE DATABASE {NameOfDatabase} FROM DISK = '{pathDB}' WITH REPLACE, RECOVERY";
                    using (SqlCommand command = new SqlCommand(restoreDB, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch
            {

            }
        }


        public bool Backup(string NameOfDatabase)
        {
            bool result = false;
            try
            {
                string appRootPath = AppContext.BaseDirectory;
                CheckConnection();
                string query = $"BACKUP DATABASE {NameOfDatabase} TO DISK = '{appRootPath}' \\backupfile.bak' WITH FORMAT,MEDIANAME = 'Z_SQLServerBackups',NAME = 'Full Backup of Testdb';";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                result = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

    }
}
