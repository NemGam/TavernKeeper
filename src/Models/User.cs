using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    /// <summary>
    /// Class that contains information about the user
    /// </summary>
    public class User
    {
        public string? UserName { get; private set; }
        public string? FirstName { get; private set; }

        public User(string userName, string firstName)
        {
            UserName = userName;
            FirstName = firstName;
        }
    }
}
