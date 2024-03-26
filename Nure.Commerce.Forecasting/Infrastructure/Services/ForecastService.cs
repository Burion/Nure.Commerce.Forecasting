using Nure.Commerce.Forecasting.Domain;
using Nure.Commerce.Forecasting.Infrastructure.Models;
using Nure.Commerce.Forecasting.Infrastructure.Models.MongoDB;

namespace Nure.Commerce.Forecasting.Infrastructure.Services
{
    public class ForecastService : IForecastService
    {
        public ForecastReport ForecastMonthlyIncome(IEnumerable<SaleHour> saleHours)
        {
            if (saleHours == null)
                throw new ArgumentNullException(nameof(saleHours));

            double estimatedIncome = (double)(saleHours.Sum(sh => sh.All * sh.B) / saleHours.Select(sh => sh.Date.Month).Distinct().Count());

            var monthlyData = 
                saleHours.GroupBy(sh => sh.Date.Month)
                .Select(
                    g => new MonthIncome() 
                    {
                        Month = g.Key, 
                        Income = (double)g.Sum(i => i.B * i.All) 
                    });

            return new ForecastReport()
            {
                MonthIncome = monthlyData,
                StartDate = saleHours.Select(sh => sh.Date).Min(),
                EndDate = saleHours.Select(sh => sh.Date).Max(),
                EstimatedMonthlyIncome = estimatedIncome
            };
        }
    }
}
