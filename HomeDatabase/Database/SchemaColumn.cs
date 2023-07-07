using Microsoft.Data.SqlClient;
using System.Data;

namespace HomeDatabase.Database
{
    public class SchemaColumn
    {
        public string ColumnName { get; set; }

        public SqlDbType DataType { get; set; }

        public int Size { get; }

        public SchemaColumn(string columnName, SqlDbType dataType)
        {
            ColumnName = columnName;
            DataType = dataType;
        }




    }
}
