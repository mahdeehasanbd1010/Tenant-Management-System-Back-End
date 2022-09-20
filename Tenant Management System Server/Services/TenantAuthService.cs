using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenant_Management_System_Server.Models;

namespace Tenant_Management_System_Server.Services
{
    public class TenantAuthService
    {
        private readonly IMongoCollection<TenantModel> _tenantAuthCollection;


        public TenantAuthService(
        IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _tenantAuthCollection = mongoDatabase.GetCollection<TenantModel>(
                databaseSettings.Value.CollectionName.Tenant_Auth);
        }

        public async Task<List<TenantModel>> GetAsync() =>
            await _tenantAuthCollection.Find(_ => true).ToListAsync();

        public async Task<TenantModel?> GetAsync(string id) =>
            await _tenantAuthCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TenantModel tenantModel) =>
            await _tenantAuthCollection.InsertOneAsync(tenantModel);

        public async Task UpdateAsync(string id, TenantModel updatedTenant) =>
            await _tenantAuthCollection.ReplaceOneAsync(x => x.Id == id, updatedTenant);

        public async Task RemoveAsync(string id) =>
            await _tenantAuthCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<TenantModel?> SignUpAsync(TenantModel tenantModel)
        {
            var tenantList = await GetAsync();
            foreach (var _tenant in tenantList)
            {
                if (_tenant.UserName == tenantModel.UserName)
                {
                    return null;
                }
            }
            await CreateAsync(tenantModel);
            return tenantModel;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var tenantList = await GetAsync();
            foreach (var _tenant in tenantList)
            {
                if (_tenant.UserName == loginModel.UserName)
                {
                    if (_tenant.Password == loginModel.Password)
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

    }
}
