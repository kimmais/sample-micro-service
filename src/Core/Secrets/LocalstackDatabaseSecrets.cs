using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;

namespace Core.Secrets
{
    public class LocalstackDatabaseSecrets : DatabaseSecrets
    {
        public LocalstackDatabaseSecrets()
        {
            _manager = new AmazonSecretsManagerClient(new AmazonSecretsManagerConfig()
            {
                ServiceURL = "http://localhost:4566"
            });
            _cache = new SecretsManagerCache(_manager);
        }
    }
}
