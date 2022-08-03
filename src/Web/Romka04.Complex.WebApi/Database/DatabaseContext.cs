using Microsoft.EntityFrameworkCore;
using Romka04.Complex.WebApi.Database.Entities;

namespace Romka04.Complex.WebApi.Database;

public sealed class DatabaseContext
    : DbContext
{
    public DbSet<ValuesEntity> Values { get; set; }

    public DatabaseContext(DbContextOptions options) 
        : base(options)
    {
        this.Database.EnsureCreated();
    }
}