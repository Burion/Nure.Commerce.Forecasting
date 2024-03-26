using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nure.Commerce.Forecasting.Domain;
using Nure.Commerce.Forecasting.Infrastructure.Models;
using Nure.Commerce.Forecasting.Infrastructure.Models.MongoDB;
using Nure.Commerce.Forecasting.Options;

namespace Nure.Commerce.Forecasting.Controllers
{
    [Route("forecast")]
    public class ForecastController : Controller
    {
        private readonly MongoClient _mongoClient;
        private readonly IForecastService _forecastService;

        public ForecastController(MongoClient mongoClient, IForecastService forecastService) 
        {
            _mongoClient = mongoClient;
            _forecastService = forecastService;
        }

        [HttpPost("populate")]
        public async Task<IActionResult> Populate()
        {
            try
            {
                _mongoClient.GetDatabase("Sales").GetCollection<SaleHour>("SalesData")
                    .InsertOne(new SaleHour() { });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SaleHour saleHour)
        {
            try
            {
                _mongoClient.GetDatabase("Sales").GetCollection<SaleHour>("SalesData")
                    .InsertOne(saleHour);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok();
        }

        [HttpPost("createmany")]
        public async Task<IActionResult> CreateMany([FromBody] SaleHour[] saleHours)
        {
            try
            {
                foreach (var sh in saleHours)
                _mongoClient.GetDatabase("Sales").GetCollection<SaleHour>("SalesData")
                    .InsertOne(sh);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ForecastYearRevenue([FromQuery] DateTime start, DateTime end, int monthToForecast)
        {
            var cachedResult = await GetCachedForecastReport(start, end, monthToForecast);

            if (cachedResult != null)
                return new OkObjectResult(cachedResult);

            var eligibleSales = (await _mongoClient.GetDatabase("Sales").GetCollection<SaleHour>("SalesData")
                .FindAsync(c => c.Date >= start
                && c.Date <= end
                )).ToList();

            var estimatedIncome = _forecastService.ForecastMonthlyIncome(eligibleSales);
            estimatedIncome.EstimatedIncome = monthToForecast * estimatedIncome.EstimatedMonthlyIncome;

            await _mongoClient.GetDatabase("Sales").GetCollection<ForecastReport>("Reports").InsertOneAsync(estimatedIncome);
            return new OkObjectResult(estimatedIncome);
        }

        private async Task<ForecastReport> GetCachedForecastReport(DateTime start, DateTime end, int monthToForecast)
        {
            var cachedResult = (await _mongoClient.GetDatabase("Sales").GetCollection<ForecastReport>("Reports")
                .FindAsync(r =>
                r.EstimatedMonthlyIncome > 0)).ToEnumerable();

            return cachedResult.FirstOrDefault(r => r.StartDate.Date == start.Date && r.EndDate.Date == end.Date);
        }
    }
}
