using Npgsql;
using System.Configuration;
using System.Diagnostics;

namespace DnDManager.Models
{
    internal class DatabaseProvider
    {
        private readonly NpgsqlConnection connection;

        
        private readonly string connectionString = 
            $@"User Id={ConfigurationManager.AppSettings.Get("UserID")};
            Password={ConfigurationManager.AppSettings.Get("Password")};
            Server={ConfigurationManager.AppSettings.Get("Server")};
            Port={ConfigurationManager.AppSettings.Get("Port")};
            Database={ConfigurationManager.AppSettings.Get("Database")}";

        public DatabaseProvider()
        {
            
            return;
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
