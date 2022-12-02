using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenant_Management_System_Server.Models;

namespace Tenant_Management_System_Server.Services
{
    public class HouseService
    {
        private readonly IMongoCollection<HomeownerModel> _homeownerAuthCollection;
        public HouseService(IOptions<DatabaseSettings> databaseSettings) 
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _homeownerAuthCollection = mongoDatabase.GetCollection<HomeownerModel>(
                databaseSettings.Value.CollectionName.Homeowner_Auth);
        }
    }
}
