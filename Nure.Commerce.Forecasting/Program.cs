using MongoDB.Driver;
using Nure.Commerce.Forecasting.Domain;
using Nure.Commerce.Forecasting.Infrastructure.Services;
using Nure.Commerce.Forecasting.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<MongoDbConfig>("MongoDB");

var configString = builder.Configuration["MongoDB:ConnectionString"];
var settings = MongoClientSettings.FromConnectionString(configString);
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);

builder.Services.AddSingleton(client);
builder.Services.AddTransient<IForecastService, ForecastService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
