using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class ShoppingItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Item { get; set; }
        public bool IsAPriority { get; set; }
        public bool IsBought { get; set; }
    }
}
