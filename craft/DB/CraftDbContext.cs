using craft.FileProvider;
using craft.Users;
using Microsoft.EntityFrameworkCore;

namespace craft.DB;

public class CraftDbContext : DbContext
{
    public DbSet<CraftUserSession> sessions { get; set; }
    public DbSet<CraftUser> users { get; set; }
    public DbSet<CraftPermission> permissions { get; set; }
    public DbSet<CraftFile> files { get; set; }
    public DbSet<FileSystemFileProvider> fileSystemFileProviders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Config.Config.LoadConfig();
        optionsBuilder.UseNpgsql(Config.Config.Instance.dbConnectionString);
    }
}