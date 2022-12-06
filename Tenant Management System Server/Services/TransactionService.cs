using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenant_Management_System_Server.Models;

namespace Tenant_Management_System_Server.Services
{
    public class TransactionService
    {
        private readonly IMongoCollection<TenantModel> _tenantAuthCollection;
        private readonly IMongoCollection<HomeownerModel> _homeownerAuthCollection;
        private readonly IMongoCollection<TransactionModel> _transactionCollection;

        public TransactionService(
        IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _tenantAuthCollection = mongoDatabase.GetCollection<TenantModel>(
                databaseSettings.Value.CollectionName.Tenant_Auth);
            _homeownerAuthCollection = mongoDatabase.GetCollection<HomeownerModel>(
                databaseSettings.Value.CollectionName.Homeowner_Auth);
            _transactionCollection = mongoDatabase.GetCollection<TransactionModel>(
                databaseSettings.Value.CollectionName.Transaction);
        }

        public async Task<List<TransactionModel>> GetAsync() =>
           await _transactionCollection.Find(_ => true).ToListAsync();

        public async Task<TransactionModel?> GetAsync(string id) =>
            await _transactionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TransactionModel transactionModel) =>
            await _transactionCollection.InsertOneAsync(transactionModel);

        public async Task UpdateAsync(string id, TransactionModel updatedTransaction) =>
            await _transactionCollection.ReplaceOneAsync(x => x.Id == id, updatedTransaction);

        public async Task RemoveAsync(string id) =>
            await _transactionCollection.DeleteOneAsync(x => x.Id == id);

    }
}
