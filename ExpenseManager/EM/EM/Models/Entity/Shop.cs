using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }

        [OneToMany]
        public List<Product> Products { get; set; }
    }
}
