using Core.Interfaces;

namespace Core.Secrets
{
    public class IdentitySecrets : Secrets, IIdentitySecrets
    {
        public string UrlIdentity()
           => GetSecrets()["url_identity"].ToObject<string>();

        protected override string SecretString()
            => "secrets/kim/identity";
    }
}
