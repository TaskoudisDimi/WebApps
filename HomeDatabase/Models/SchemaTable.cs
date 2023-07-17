namespace HomeDatabase.Models
{
    public class SchemaTable
    {

        public string TableName { get; set; }

        public List<SchemaColumn> Columns { get; set; }


        public SchemaTable(string tableName, List<SchemaColumn> columns)
        {
            TableName = tableName;
            Columns = columns;
        }

    }
}
