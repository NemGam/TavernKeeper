using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    internal class GameModel
    {
        public long campaign_id {  get; set; }
        public string created_by {  get; set; }
        public string created_at { get; set; }
        public string campaign_name { get; set; }
        public string password { get; set; }
        public string pass_salt { get; set; }

        public GameModel()
        {
            
        }
    }
}
