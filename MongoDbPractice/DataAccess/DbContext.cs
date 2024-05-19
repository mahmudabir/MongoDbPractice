using Microsoft.EntityFrameworkCore;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

using MongoDbPractice.DataAccess.Models;

internal class MyDbContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Customer> Customers { get; init; }
    public MyDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().ToCollection("Customers");
        modelBuilder.Entity<Customer>().HasKey(x => x.Id);
        modelBuilder.Entity<Customer>().Property(x => x.Id).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.UserId).IsRequired();
        modelBuilder.Entity<Customer>().HasOne(x => x.User).WithOne(x => x.Customer).IsRequired();

        modelBuilder.Entity<User>().HasKey(x => x.Id);
        //modelBuilder.Entity<User>().HasIndex(x => x.Name).IsUnique(); //Does not work in 8.0.0
        modelBuilder.Entity<User>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<User>().HasOne(x => x.Customer).WithOne(x => x.User).IsRequired();
    }
}