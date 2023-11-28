using DnDManager.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

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
            Debug.WriteLine(connectionString);
            connection = GetConnection(connectionString);
            try
            {
                OpenConnection(connection);
            }
            catch (Npgsql.PostgresException e)
            {
                Debug.WriteLine(e.Message, "Error");
            }
            
            return;
        }

        private async Task OpenConnection(NpgsqlConnection connection)
        {
            await connection.OpenAsync();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Debug.WriteLine("Successfully Connected to the database");
            }
        }

        public List<IDbReturnable> GetFromDatabase(IDbCommand dbCommand)
        {
            List<IDbReturnable> listToReturn = new List<IDbReturnable>();
            //Do the magic, fill the list, return it
            return listToReturn;
        }

        ~DatabaseProvider() 
        {
            Debug.WriteLine("Closing connection");
            connection.Close();
        }

        private static NpgsqlConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
