using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PACS.Models;

namespace PACS.DB
{
    public partial class pacsContext : DbContext
    {
        public pacsContext()
        {
        }

        public pacsContext(DbContextOptions<pacsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;
        public virtual DbSet<Point> Points { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=pacs;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.Dec).HasMaxLength(50);

                entity.Property(e => e.DirName).HasMaxLength(50);

                entity.Property(e => e.Hex).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PassOrDeny).HasMaxLength(50);

                entity.Property(e => e.PointId).HasColumnName("PointID");

                entity.Property(e => e.Position).HasMaxLength(50);

                entity.Property(e => e.Time).HasMaxLength(50);

                entity.Property(e => e.TimeConverted).HasColumnType("datetime");

                entity.Property(e => e.W26).HasMaxLength(50);

                entity.HasOne(d => d.Point)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.PointId)
                    .HasConstraintName("FK_Event_Point");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("Personal");

                entity.Property(e => e.Fio)
                    .HasMaxLength(100)
                    .HasColumnName("FIO");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("Point");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
