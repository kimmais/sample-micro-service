namespace Core
{
    public interface IDatabaseSecrets
    {
        public string Database();
        public string Host();
        public string Username();
        public string Port();
        public string Password();
    }
}
