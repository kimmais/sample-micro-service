using Core;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class SampleContext : DbContext
    {
        private readonly IDatabaseSecrets _secrets;

        public SampleContext(IDatabaseSecrets secrets)
        {
            _secrets = secrets;

        }
        //Insira os DbSets aqui ex:  public DbSet<Example> Examples { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=" + _secrets.Host() + ";Port=" + _secrets.Port() +
                ";Database=" + _secrets.Database() + ";Username=" + _secrets.Username() + ";Password=" + _secrets.Password());
        }
    }
}
