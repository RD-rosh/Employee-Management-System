using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Employee_Management_System.Models;

public partial class EmployeeDbContext : DbContext
{
   
    public EmployeeDbContext()
    {

    }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Employees_pkey");

            entity.HasIndex(e => e.Id, "unique_employee_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.JobPosition)
                .HasMaxLength(150)
                .HasColumnName("jobPosition");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PublicHoliday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PublicHolidays_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HolidayDate).HasColumnName("holidayDate");
            entity.Property(e => e.HolidayName)
                .HasMaxLength(300)
                .HasColumnName("holidayName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
