using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Value { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
