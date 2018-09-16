using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2SqliteConverter
{
    class Excel2SqliteConverter
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

        public Excel2SqliteConverter(string dataSource, string tableName)
        {
            DataSource = dataSource;
            TableName = tableName;
        }

        private void CreateTableFromDataRow(DataColumnCollection columns)
        {
            var joinedColumns = string.Join(
                ", ",
                GetColumnNames(columns)
                    .Select(name => $"{name} VARCHAR(30);")
            );

            var query = $"CREATE TABLE {TableName} ({joinedColumns});";
            ExecuteQuery(query);
        }

        private static IEnumerable<string> GetColumnNames(DataColumnCollection columns)
        {
            return columns
                .Cast<DataColumn>()
                .Select(col => col.ColumnName);
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
    }
}
