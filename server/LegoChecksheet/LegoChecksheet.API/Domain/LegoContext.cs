using LegoChecksheet.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LegoChecksheet.API.Domain;

public class LegoContext : DbContext
{
    public LegoContext(DbContextOptions<LegoContext> opts): base(opts)
    {
        
    }

    public DbSet<LegoSet> LegoSets { get; set; }

    public DbSet<LegoPiece> LegoPieces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LegoSet>()
            .HasKey(x => x.LegoSetId);

        modelBuilder.Entity<LegoSet>()
            .Property(x => x.LegoSetId)
            .ValueGeneratedNever();
        
        modelBuilder.Entity<LegoPiece>()
            .HasKey(x => x.LegoPieceId);

        modelBuilder.Entity<LegoPiece>()
            .Property(x => x.LegoPieceId)
            .ValueGeneratedNever();
        
        base.OnModelCreating(modelBuilder);
    }
}