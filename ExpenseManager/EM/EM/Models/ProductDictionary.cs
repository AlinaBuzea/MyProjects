using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models
{
    public class ProductDictionary : IEquatable<ProductDictionary>
    {
        public string ProdName { get; set; }
        public string ProdCategory { get; set; }

        public ProductDictionary() { }

        public ProductDictionary(string productName, string productCategory)
        {
            ProdName = productName;
            ProdCategory = productCategory;
        }

        public bool Equals(ProductDictionary other)
        {
            return ProdName.Equals(other.ProdName) && ProdCategory.Equals(other.ProdCategory);
        }

        override
        public string ToString()
        {
            return ProdName + " " + ProdCategory;
        }
    }
}
