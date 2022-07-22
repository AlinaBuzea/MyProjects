using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EM.ViewModels.SubViewModels
{
    public class MonthYearVM : BaseViewModel
    {
        public enum MonthEnum
        {
            Ianuarie,
            Februarie,
            Martie,
            Aprilie,
            Mai,
            Iunie,
            Iulie,
            August,
            Septembrie,
            Octombrie,
            Noiembrie,
            Decembrie
        }

        private readonly int minYear;
        private readonly int maxYear;

        private List<string> months;
        private List<int> years;
        private string selectedMonth;
        private int selectedYear;

        public MonthYearVM()
        {
            minYear = 2010;
            maxYear = DateTime.Today.Year;
            Months = new List<string>(Enum.GetNames(typeof(MonthEnum)));
            Years = new List<int>(Enumerable.Range(minYear, maxYear - minYear + 1));
        }

        #region Properties
        public List<string> Months
        {
            get => months;
            set => SetProperty(ref months, value);
        }

        public List<int> Years
        {
            get => years;
            set => SetProperty(ref years, value);
        }

        public string SelectedMonth
        {
            get => selectedMonth;
            set => SetProperty(ref selectedMonth, value);
        }

        public int SelectedYear
        {
            get => selectedYear;
            set => SetProperty(ref selectedYear, value);
        }
        #endregion


        public MonthEnum ConvertStringToMonthEnum(string monthString)
        {
            return (MonthEnum)Enum.Parse(typeof(MonthEnum), monthString);
        }

        public string ConvertMonthEnumToString(MonthEnum monthEnum)
        {
            return Enum.GetName(typeof(MonthEnum), monthEnum);
        }

    }
}
