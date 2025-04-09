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

    public virtual DbSet<Termin> Termin { get; set; }

    public virtual DbSet<TerminPlayers> TerminPlayers { get; set; }

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
            entity.Property(e => e.Foot)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sex)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Termin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Termini_Termin_Id");

            entity.ToTable("Termin", "Termini");

            entity.HasIndex(e => new { e.ScheduledDate, e.StartTime }, "IX_Termini_Termin_ScheduledDate_StartTime").HasFilter("([Active]=(1))");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TerminPlayers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Termini_Termin_Players_Id");

            entity.ToTable("Termin.Players", "Termini");

            entity.HasIndex(e => e.PlayerId, "IX_Termini_Termin_PlayerId").HasFilter("([Active]=(1))");

            entity.HasIndex(e => e.TerminId, "IX_Termini_Termin_TerminId").HasFilter("([Active]=(1))");

            entity.HasIndex(e => new { e.TerminId, e.PlayerId }, "IX_Termini_Termin_Termin_with_player").HasFilter("([Active]=(1))");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Player).WithMany(p => p.TerminPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termini_Termin_Players_PlayerId");

            entity.HasOne(d => d.Termin).WithMany(p => p.TerminPlayers)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termini_Termin_Players_TerminId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
