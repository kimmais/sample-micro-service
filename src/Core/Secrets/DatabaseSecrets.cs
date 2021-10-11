namespace Core.Secrets
{
    public class DatabaseSecrets : Secrets, IDatabaseSecrets
    {
        public string Database()
                => GetSecrets()["database"].ToObject<string>();

        public string Host()
            => GetSecrets()["host"].ToObject<string>();

        public string Username()
            => GetSecrets()["username"].ToObject<string>();

        public string Port()
            => GetSecrets()["port"].ToObject<string>();

        public string Password()
            => GetSecrets()["password"].ToObject<string>();

        protected override string SecretString()
            => "secrets/kim/database/01";
    }
}
