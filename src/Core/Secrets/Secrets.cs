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
            _manager = new AmazonSecretsManagerClient();
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
