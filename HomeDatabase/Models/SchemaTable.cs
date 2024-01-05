using System.Data;

namespace HomeDatabase.Models
{
    public class SchemaTable
    {
        public string TableName { get; }
        public List<SchemaColumn> Columns { get; }

        public SchemaTable(string tableName, List<SchemaColumn> columns)
        {
            TableName = tableName;
            Columns = columns;
        }
    }


}
