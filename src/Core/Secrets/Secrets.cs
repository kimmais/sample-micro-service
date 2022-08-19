using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Newtonsoft.Json.Linq;
using System;

namespace Core.Secrets
{
    public abstract class Secrets : IDisposable
    {

        protected AmazonSecretsManagerClient _manager;
        protected SecretsManagerCache _cache;

        public Secrets()
        {
#if DEBUG
            var config = new AmazonSecretsManagerConfig()
            { ServiceURL = "http://localhost:4566" };
            _manager = new AmazonSecretsManagerClient(config);
#else
                _manager = new AmazonSecretsManagerClient();
#endif
            _cache = new SecretsManagerCache(_manager);
        }

        public void Dispose()
        {
            _manager.Dispose();
            _cache.Dispose();
        }


        protected JObject GetSecrets()
        {
            var sec = _cache.GetSecretString(SecretString()).Result;
            return JObject.Parse(sec);
        }


        protected abstract string SecretString();

    }
}

