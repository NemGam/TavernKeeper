using DnDManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Stores
{
    /// <summary>
    /// Class that contains information about the current user
    /// </summary>
    public class UserStore
    {
        private User _currentUser; 

        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }
    }
}
