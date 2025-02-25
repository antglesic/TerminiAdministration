using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerminiDataAccess.TerminiContext.Models;

namespace TerminiDataAccess.TerminiContext;

public partial class TerminiContext : DbContext
{
    public TerminiContext(DbContextOptions<TerminiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Player { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Termini_Player_Id");

            entity.ToTable("Player", "Termini");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
