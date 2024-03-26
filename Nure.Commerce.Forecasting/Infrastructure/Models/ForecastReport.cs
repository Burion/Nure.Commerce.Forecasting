using MongoDB.Bson.Serialization.Attributes;

namespace Nure.Commerce.Forecasting.Infrastructure.Models
{
    [BsonIgnoreExtraElements]
    public class ForecastReport
    {
        public IEnumerable<MonthIncome> MonthIncome { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double EstimatedMonthlyIncome { get; set; }
        public double EstimatedIncome { get; set; }
    }
}
