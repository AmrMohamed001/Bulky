using Bulky.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bulky.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt) {}
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasData(
        new Category {Id = 1,Name = "test1",DisplayOrder = 1},
        new Category {Id = 2,Name = "test2",DisplayOrder = 2},
        new Category {Id = 3,Name = "test3",DisplayOrder = 3});
    }
}