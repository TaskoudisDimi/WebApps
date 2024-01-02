using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClass
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
