using System.Linq.Expressions;
using CloudApp.Utils;
using CloudAppWebApi.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CloudAppWebApi.Repository
{
    public class MongoDbRepository : IDbRepository
    {
        private readonly IMongoDatabase _database;
        private const string CloudTableName = "Cloud";

        public MongoDbRepository()
        {
            var connString = AppSettings.ConnectionString;
            var dbClient = new MongoClient(connString);
            _database = dbClient.GetDatabase("data");
        }

        public async Task<IEnumerable<IFile>> GetFiles()
        {
            var collection = _database.GetCollection<BsonDocument>(CloudTableName);
            var results = await collection.FindAsync(_ => true);
            return results.ToList().Select(r => BsonSerializer.Deserialize<CloudFile>(r));
        }

        public IFile? GetDetails(string Id)
        {
            var collection = _database.GetCollection<CloudFile>(CloudTableName);
            return collection?.Find(d => d.Id == Id)?.ToList()?.FirstOrDefault();
        }

        public Task<IFile> Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<IFile> DownloadFile(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task UploadFileAsync(IFile file)
        {
            var dataTable = _database.GetCollection<BsonDocument>(CloudTableName);
            //var textFile = new TextFile
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Title = file.Title,
            //    IsChecked = false,
            //    Content = file.Content.ToArray()
            //};

            var fileAsBson = file.ToBsonDocument();
            await dataTable.InsertOneAsync(fileAsBson);
        }

        //public IEnumerable<StrategyResultsDto> GetStrategyInfo(string strategyName, string currency, string hour)
        //{
        //   var ResultsInfo = _db.GetCollection<BsonDocument>(ResultsInfoTableName);
        //   //var filter = Builders<StrategyResultsDto>.Filter.Where(e => e.StrategyName == strategyName).ToBsonDocument();
        //   //&& e.Currency.Replace("/", "") == currency && e.Period.ToString().Replace(" ", "") == hour).ToBsonDocument();

        //   var filter = Builders<BsonDocument>.Filter.Eq("StrategyName", strategyName);
        //   var results = ResultsInfo.Find(filter).ToList();
        //   var resultAsList = results.Select(r => BsonSerializer.Deserialize<StrategyResultsDto>(r));
        //   return resultAsList.Where(r => r.Currency.Replace("/", "").Equals(currency, StringComparison.InvariantCultureIgnoreCase)
        //                        && r.Timeframe.Replace(" ", "").Equals(hour, StringComparison.InvariantCultureIgnoreCase));
        //}

        //public IEnumerable<TradeLogs> GetOrders(string strategyName, string currency, string hour)
        //{
        //   var ordersTable = _db.GetCollection<BsonDocument>(TradeLogsTableName);
        //   List<BsonDocument> results;

        //   results = ordersTable.Find(_ => true).ToList();
        //   //var filter = Builders<BsonDocument>.Filter.Eq("LinkNumber", id);
        //   //results = ordersTable.Find(filter).ToList();
        //   var ordersAsList = results.Select(r => BsonSerializer.Deserialize<TradeLogs>(r));
        //   return ordersAsList;
        //}

        //  public IEnumerable<StrategyTemplate> GetStrategyNamesDistinct()
        //  {
        //      //var ordersTable = _db.GetCollection<BsonDocument>(OrdersTableName);
        //      //var results = ordersTable.Find(_ => true).ToList();
        //      //var ordersAsList = results.Select(r => BsonSerializer.Deserialize<HistoricalOrders>(r));

        //      //return ordersAsList.Select(e => e.StrategyName).Distinct().Select(e => new StrategyTemplate(e));
        //      return GetFieldValue<TradeLogs, string>(x => x.StrategyName).Distinct().Select(x => new StrategyTemplate(x));
        //  }

        //  public async Task<StartEndDateTemplate> GetStartEndDates(string strategyName)
        //  {
        //      var startDate = await GetFieldValue<TradeLogs, DateTime>(strategyName, x => x.Orders.Select(o => o.OpenDate).Min());
        //      var lastDate = await GetFieldValue<TradeLogs, DateTime>(strategyName, x => x.Orders.Select(o => o.CloseDate).Max());
        //      return new StartEndDateTemplate(startDate, lastDate);
        //  }



        //  public async Task<StartEndDateTemplate> GetStartEndDates(string strategyName, string currency, string timeFrame)
        //  {
        //      var startDate = await GetFieldValue<TradeLogs, DateTime>(strategyName, currency, timeFrame, x => x.Orders.Select(o => o.OpenDate).Min());
        //      var lastDate = await GetFieldValue<TradeLogs, DateTime>(strategyName, currency, timeFrame, x => x.Orders.Select(o => o.CloseDate).Max());
        //      return new StartEndDateTemplate(startDate, lastDate);
        //  }

        //  public async Task<DateTime> GetStartDate(string strategyName, string currency, string timeFrame)
        //  {
        //      return await GetFieldValue<TradeLogs, DateTime>(strategyName, currency, timeFrame, x => x.Orders.Select(o => o.OpenDate).Min());
        //  }

        //  public async Task<DateTime> GetEndDate(string strategyName, string currency, string timeFrame)
        //  {
        //      return await GetFieldValue<TradeLogs, DateTime>(strategyName, currency, timeFrame, x => x.Orders.Select(o => o.CloseDate).Max());
        //  }

        //  public IEnumerable<TradeLogs> GetOrdersByStrategyName(string strategyName)
        //  {
        //      return _db.GetCollection<TradeLogs>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName)
        //          .Project(new ProjectionDefinitionBuilder<TradeLogs>().Expression(x => x)).ToList();
        //  }

        //  public IEnumerable<TradeLogs> GetOrdersByStrategyName(string strategyName, string currency, string timeFrame)
        //  {
        //      return _db.GetCollection<TradeLogs>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName && d.Currency == currency && d.TimeFrame == timeFrame)
        //          .Project(new ProjectionDefinitionBuilder<TradeLogs>().Expression(x => x)).ToList();
        //  }



        //  //this is not needed
        //  //public IEnumerable<StrategyResultsDto> GetStrategyInfoByStrategyName(string strategyName)
        //  //{
        //  //    return _db.GetCollection<StrategyResultsDto>(ResultsInfoTableName)
        //  //        .Find(d => d.StrategyName == strategyName)
        //  //        .Project(new ProjectionDefinitionBuilder<StrategyResultsDto>().Expression(x => x)).ToList();
        //  //}

        //  private async Task<TValue> GetFieldValue<TEntity, TValue>(string strategyName, Expression<Func<TEntity, TValue>> fieldExpression) where TEntity : TradeLogs
        //  {
        //      var propertyValue = await _db.GetCollection<TEntity>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName)
        //          .Project(new ProjectionDefinitionBuilder<TEntity>().Expression(fieldExpression))
        //          .FirstOrDefaultAsync();

        //      return propertyValue;
        //  }

        //  private async Task<TValue> GetFieldValue<TEntity, TValue>(string strategyName, string currency, string timeFrame, Expression<Func<TEntity, TValue>> fieldExpression) where TEntity : TradeLogs
        //  {
        //      var propertyValue = await _db.GetCollection<TEntity>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName && d.Currency == currency && d.TimeFrame == timeFrame)
        //          .Project(new ProjectionDefinitionBuilder<TEntity>().Expression(fieldExpression))
        //          .FirstOrDefaultAsync();

        //      return propertyValue;
        //  }

        //  private IEnumerable<TValue> GetFieldValue<TEntity, TValue>(Expression<Func<TEntity, TValue>> fieldExpression) where TEntity : TradeLogs
        //  {
        //      var propertyValue = _db.GetCollection<TEntity>(TradeLogsTableName)
        //          .Find(d => true)
        //          .Project(new ProjectionDefinitionBuilder<TEntity>().Expression(fieldExpression)).ToList();

        //      return propertyValue;
        //  }

        //  public IEnumerable<string> GetCurrenciesDistinct(string strategyName)
        //  {
        //      return _db.GetCollection<TradeLogs>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName)
        //          .Project(new ProjectionDefinitionBuilder<TradeLogs>().Expression(x => x.Currency)).ToList().Distinct();
        //  }

        //  public IEnumerable<string> GetTimeFramesDistinct(string strategyName)
        //  {
        //      return _db.GetCollection<TradeLogs>(TradeLogsTableName)
        //          .Find(d => d.StrategyName == strategyName)
        //          .Project(new ProjectionDefinitionBuilder<TradeLogs>().Expression(x => x.TimeFrame)).ToList().Distinct();
        //  }
    }
}
