using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace Nure.Commerce.Forecasting.Infrastructure.Models.MongoDB
{
    [BsonIgnoreExtraElements]
    public class SaleHour
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("date")]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public DateTime Date { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("B")]
        public int? B { get; set; }
        [JsonPropertyName("All")]
        public int? All {  get; set; }
    }
}
