using System.Globalization;
using System.Transactions;

namespace Nure.Commerce.Forecasting.Infrastructure.Models
{
    public class MonthIncome
    {
        public int Month { get; set; }
        public string MonthString => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        public double Income { get; set; }
    }
}
