using Npgsql;
using System.Diagnostics;

namespace DnDManager.Models
{
    internal class DatabaseProvider
    {
        private readonly NpgsqlConnection connection;

        private readonly string connectionString = @"Server=localhost;Port=5432;User ID=postgres;Password=;Database=Test;";

        private DatabaseProvider()
        {
            connection = GetConnection();
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Debug.WriteLine("WEOGJWEOGJWEJWEJGKIJWEGIJNWEJG");
            }
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
