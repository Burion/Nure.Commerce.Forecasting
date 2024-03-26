using Nure.Commerce.Forecasting.Infrastructure.Models;
using Nure.Commerce.Forecasting.Infrastructure.Models.MongoDB;

namespace Nure.Commerce.Forecasting.Domain
{
    public interface IForecastService
    {
        ForecastReport ForecastMonthlyIncome(IEnumerable<SaleHour> saleHours);
    }
}
