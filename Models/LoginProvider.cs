namespace DnDManager.Models
{
    internal class LoginProvider
    {

        public LoginProvider()
        {


        }


        public bool CheckAuthentication(string login, string password)
        {
            return false;
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
