namespace DnDManager.Models
{
    internal class LoginProvider
    {
        private readonly DatabaseProvider _databaseProvider;
        public LoginProvider(DatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;

        }

        public bool CheckAuthentication(string login, string password)
        {
            return true;
            /*
            //Check if any fields are empty
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            //Debug.WriteLine($"{Login}, {Password}");
            string SQL = $"SELECT password FROM sample_data WHERE username = '{login}'";
            var cmd = new NpgsqlCommand(SQL, connection);

            var reader = cmd.ExecuteReader();

            return reader.GetString(0) == password;
            */
        }

    }
}
