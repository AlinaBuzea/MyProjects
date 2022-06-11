using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int OrderIndex { get; set; }

        [OneToMany]
        public List<Product> Products { get; set; }

        [OneToMany]
        public List<Budget> Budgets { get; set; }
    }
}
