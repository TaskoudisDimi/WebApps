using HomeDatabase.Models;

namespace HomeDatabase.Database
{
    public class CheckSchemaDB
    {

        public void CheckSchema(List<SchemaTable> Table)
        {
            var tables = Table;

            foreach(var table in tables)
            {
                if (!TableExists())
                {
                    //Create
                }
                else
                {
                    //Update
                }
            }


        }

        public void CheckTable()
        {

        }

        public bool TableExists()
        {
            return true;
        }


    }
}
