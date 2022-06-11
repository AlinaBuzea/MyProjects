using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPIN { get; set; }
    }
}
