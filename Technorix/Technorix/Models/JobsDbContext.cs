using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Technorix.Models;

public partial class JobsDbContext : DbContext
{
    public JobsDbContext()
    {
    }

    public JobsDbContext(DbContextOptions<JobsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
    

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DEPARTME__3214EC2715BEFA0E");

            entity.ToTable("DEPARTMENTS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JOBS__3214EC27C1F6D7F6");

            entity.ToTable("JOBS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Closingdate)
                .HasColumnType("datetime")
                .HasColumnName("CLOSINGDATE");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.Departmentid).HasColumnName("DEPARTMENTID");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Locationid).HasColumnName("LOCATIONID");
            entity.Property(e => e.Posteddate)
                .HasColumnType("datetime")
                .HasColumnName("POSTEDDATE");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.Department).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JOBS__DEPARTMENT__15502E78");

            entity.HasOne(d => d.Location).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.Locationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JOBS__LOCATIONID__145C0A3F");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LOCATION__3214EC27CF283BFF");

            entity.ToTable("LOCATIONS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.Zip).HasColumnName("ZIP");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERS__3214EC2703639AA2");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Username, "UQ__USERS__B15BE12EE8938D6A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATEDATE");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORDHASH");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Userrole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
