using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tenant_Management_System_Server.Models;

namespace Tenant_Management_System_Server.Services
{
    public class HomeownerAuthService
    {
        private readonly IMongoCollection<HomeownerModel> _homeownerAuthCollection;

        public HomeownerAuthService(
        IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _homeownerAuthCollection = mongoDatabase.GetCollection<HomeownerModel>(
                databaseSettings.Value.CollectionName.Homeowner_Auth);
        }

        public async Task<List<HomeownerModel>> GetAsync() =>
            await _homeownerAuthCollection.Find(_ => true).ToListAsync();

        public async Task<HomeownerModel?> GetAsync(string id) =>
            await _homeownerAuthCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(HomeownerModel homeowner) =>
            await _homeownerAuthCollection.InsertOneAsync(homeowner);

        public async Task UpdateAsync(string id, HomeownerModel updatedHomeowner) =>
            await _homeownerAuthCollection.ReplaceOneAsync(x => x.Id == id, updatedHomeowner);

        public async Task RemoveAsync(string id) =>
            await _homeownerAuthCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<HomeownerModel?> SignUpAsync(HomeownerModel homeowner) {
            var homeownerList = await GetAsync();
            foreach (var _homeowner in homeownerList)
            {
                if(_homeowner.UserName == homeowner.UserName)
                {
                    return null;
                }
            }
            await CreateAsync(homeowner);
            return homeowner;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            var homeownerList = await GetAsync();
            foreach (var _homeowner in homeownerList)
            {
                if (_homeowner.UserName == loginModel.UserName)
                {
                    if (_homeowner.Password == loginModel.Password)
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
