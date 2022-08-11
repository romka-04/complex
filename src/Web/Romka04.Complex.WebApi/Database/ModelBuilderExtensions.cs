using Microsoft.EntityFrameworkCore;
using Romka04.Complex.WebApi.Database.Entities;

namespace Romka04.Complex.WebApi.Database;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        //Department
        modelBuilder.Entity<ValuesEntity>()
            .HasData(
                new ValuesEntity { Number = 1 },
                new ValuesEntity { Number = 19 }
            );
    }
}