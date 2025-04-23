using KingJwtAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingJwtAuth.Context
{
    public interface IDataBaseContext
    {
        DbSet<UserToken> UserToken { get; set; }
        DbSet<RefreshToken> RefreshToken { get; set; }
        //SaveChanges
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
    }
    public class DataBaseContext :DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
