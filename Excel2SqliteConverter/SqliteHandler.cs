using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2SqliteConverter
{
    class SqliteHandler
    {
        public string TableName { get; }
        public string DataSource { get; }

        public SQLiteConnection Connection
        {
            get
            {
                var connection = new SQLiteConnection($"Data Source={DataSource};");
                connection.Open();
                return connection;
            }
        }

        public SqliteHandler(string dataSource, string tableName)
        {
            DataSource = dataSource;
            TableName = tableName;
        }

        private void ExecuteQuery(string query)
        {
            using (var connection = Connection)
            {
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetDataTable()
        {
            var query = $"SELECT * FROM {TableName}";

            using (var conneciton = Connection)
            {
                using (var command = new SQLiteCommand(query, conneciton))
                {
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);

                        return table;
                    }
                }
            }
        }
    }
}
