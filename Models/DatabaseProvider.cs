using Dapper;
using DnDManager.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    internal class DatabaseProvider
    {
        private readonly NpgsqlDataSource _dataSource;
        private readonly string _connectionString;

        /// <summary>
        /// Creates a new <see cref="DatabaseProvider"/>
        /// </summary>
        /// <returns></returns>
        public static DatabaseProvider Create()
        {
            string connectionString =
            $@"User Id={ConfigurationManager.AppSettings.Get("UserID")};
            Password={ConfigurationManager.AppSettings.Get("Password")};
            Server={ConfigurationManager.AppSettings.Get("Server")};
            Port={ConfigurationManager.AppSettings.Get("Port")};
            Database={ConfigurationManager.AppSettings.Get("Database")}";
            var source = NpgsqlDataSource.Create(connectionString);
            return new DatabaseProvider(source, connectionString);
        }

        private DatabaseProvider(NpgsqlDataSource dataSouce, string connectionString)
        {
            _dataSource = dataSouce;
            _connectionString = connectionString;
            return;
        }


        /// <summary>
        /// Call procedure stored in the database.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CallProcedureAsync(string name, object? param = null)
        {
            using (var connection = await _dataSource.OpenConnectionAsync())
            {
                await connection.ExecuteAsync(name, param,
                    commandType: CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// Call procedure stored in the database.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<T>>? CallProcedureAsync<T>(string name, object? param = null)
        {
            List<T>? result;
            using (var connection = await _dataSource.OpenConnectionAsync())
            {
                result = (List<T>)await connection.QueryAsync<T>(name, param,
                    commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /// <summary>
        /// Send a request to manipulate data in the database.
        /// Use '@[YOUR PARAM NAME]' to parametrize the statement. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">(Parametrized) sql statement to execute.</param>
        /// <param name="param">Parameters to replace '@[YOUR PARAM NAME]' with.</param>
        public async Task PostAsync(string sql, object? param = null)
        {
            using (var connection = await _dataSource.OpenConnectionAsync())
            {
                await connection.ExecuteAsync(sql, param);
            }
        }

        /// <summary>
        /// Send a request to retrieve data of type <typeparamref name="T"/> from the database.
        /// Use '@[YOUR PARAM NAME]' to parametrize the statement. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">(Parametrized) sql statement to execute.</param>
        /// <param name="param">Parameters to replace '@[YOUR PARAM NAME]' with.</param>
        /// <returns> <see cref="List{T}"/> of converted objects of type <typeparamref name="T"/>.</returns>
        public async Task<List<T>> GetAsync<T>(string sql, object? param = null)
        {
            List<T> result;
            using (var connection = await _dataSource.OpenConnectionAsync())
            {
                Debug.WriteLine(connection.Database);
                result = (List<T>)await connection.QueryAsync<T>(sql, param);
            }
            
            return result;
        }
    }
}
