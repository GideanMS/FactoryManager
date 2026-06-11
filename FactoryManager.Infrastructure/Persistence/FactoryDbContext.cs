using Microsoft.EntityFrameworkCore;

public class FactoryDbContext : DbContext
{
    public FactoryDbContext(
        DbContextOptions<FactoryDbContext> options)
        : base (options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<Machine> Machines => Set<Machine>();

    public DbSet<Recipe> Recipes => Set<Recipe>();
}