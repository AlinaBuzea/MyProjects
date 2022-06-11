using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models.Entity
{
    public class Budget
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double AlocatedBudget { get; set; }
        public double CurrentValue { get; set; }
        public double LimitNotificationValue { get; set; }
        //public DateTime MonthYear { get; set; } /// TODO de modificat aici cu ce e comentat
        public string Month { get; set; }
        public int Year { get; set; }

        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }

        [ManyToOne]
        public Category BudgetCategory { get; set; }
    }
}
