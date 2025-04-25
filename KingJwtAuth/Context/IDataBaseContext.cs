using KingJwtAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingJwtAuth.Context
{
    public interface IDataBaseContext
    {
        DbSet<Users> Users { get; set; }
        DbSet<UserLogs> UserLogs { get; set; }
        DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        //SaveChanges
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
    }
    public class DataBaseContext :DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserLogs> UserLogs { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
