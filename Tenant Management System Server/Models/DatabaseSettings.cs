namespace Tenant_Management_System_Server.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public DatabaseCollectionModel CollectionName { get; set; } = null!;
    }
}
