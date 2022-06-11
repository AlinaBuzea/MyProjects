using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public DateTime AquisitionDate { get; set; }
        public double Price { get; set; }
        public bool IsMarked { get; set; }

        [ForeignKey(typeof(Category))]
        public int ProdCategoryId { get; set; }

        [ForeignKey(typeof(Shop))]
        public int AquisitionShopId { get; set; }

        [ManyToOne]
        public Category ProductCategory { get; set; }

        [ManyToOne]
        public Shop Shop { get; set; }
    }
}
