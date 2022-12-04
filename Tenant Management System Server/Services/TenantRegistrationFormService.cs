using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Models.TenantRegistrationForm;

namespace Tenant_Management_System_Server.Services
{
    public class TenantRegistrationFormService
    {
        private readonly IMongoCollection<TenantRegistrationFormModel> _tenantRegistrationFormCollection;

        public TenantRegistrationFormService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _tenantRegistrationFormCollection = mongoDatabase.GetCollection<TenantRegistrationFormModel>(
                databaseSettings.Value.CollectionName.Tenant_Registration_Form);
        }

        public async Task<TenantRegistrationFormModel?> GetAsync(string id) =>
            await _tenantRegistrationFormCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<TenantRegistrationFormModel> GetByUserNameAsync(string? username) =>
            await _tenantRegistrationFormCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();

        public async Task CreateAsync(TenantRegistrationFormModel tenantRegistrationFormModel) =>
            await _tenantRegistrationFormCollection.InsertOneAsync(tenantRegistrationFormModel);

        public async Task UpdateAsync(string id, TenantRegistrationFormModel updatedTenantRegistrationFormModel) =>
            await _tenantRegistrationFormCollection.ReplaceOneAsync(x => x.Id == id, updatedTenantRegistrationFormModel);

        public async Task RemoveAsync(string id) =>
            await _tenantRegistrationFormCollection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveByUserNameAsync(string username) =>
            await _tenantRegistrationFormCollection.DeleteOneAsync(x => x.UserName == username);

        public async Task<TenantRegistrationFormModel?> SaveInfoAsync(TenantRegistrationFormModel tenantRegistrationFormModel)
        {
            await CreateAsync(tenantRegistrationFormModel);
            return tenantRegistrationFormModel;
        }
    }
}
