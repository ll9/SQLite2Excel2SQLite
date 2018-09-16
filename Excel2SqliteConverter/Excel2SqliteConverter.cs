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

        private static IEnumerable<string> GetColumnNames(DataColumnCollection columns)
        {
            return columns
                .Cast<DataColumn>()
                .Select(col => col.ColumnName);
        }

        public void CreateAndFillDb(DataTable dataTable)
        {
            CreateTable(dataTable.Columns);
            InsertToDb(dataTable);
        }

        private void CreateTable(DataColumnCollection columns)
        {
            var joinedColumns = GetColumnNames(columns)
                .Select(name => $"{name} VARCHAR(30);")
                .Aggregate((current, next) => $"{current}, {next}");


            var query = $"CREATE TABLE {TableName} ({joinedColumns});";
            ExecuteQuery(query);
        }

        private void InsertToDb(DataTable dataTable)
        {
            var table = dataTable.AsEnumerable();
            var colNames = GetColumnNames(dataTable.Columns).ToList();

            var inserts = table
                .Skip(1)
                .Select(row => GetRowValues(row, colNames))
                .Aggregate((current, next) => $"({current}), ({next})");

            var query = $"INSERT INTO {TableName} VALUES {inserts};";
            ExecuteQuery(query);
        }

        private static string GetRowValues(DataRow row, IEnumerable<string> colNames)
        {
            return colNames
                .Select(name => row[name])
                .Aggregate((current, next) => current + ", " + next)
                .ToString();
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
