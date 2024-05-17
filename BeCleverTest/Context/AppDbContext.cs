using BeCleverTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BeCleverTest;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {

    }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Register> Registers { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql("Name=ConnectionStrings:BeClever");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.IdBusiness).HasName("Business_pkey");

            entity.ToTable("Business", "BeClever");

            entity.Property(e => e.IdBusiness).HasColumnName("idBusiness");
            entity.Property(e => e.LocationName)
                .HasMaxLength(100)
                .HasColumnName("location_name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdDepartment).HasName("Departments_pkey");

            entity.ToTable("Departments", "BeClever");

            entity.Property(e => e.IdDepartment).HasColumnName("idDepartment");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("Employees_pkey");

            entity.ToTable("Employees", "BeClever");

            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .HasColumnName("gender");
            entity.Property(e => e.IdDepartments).HasColumnName("idDepartments");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");

            entity.HasOne(d => d.IdDepartmentsNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdDepartments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DEPARTMENT");
        });

        modelBuilder.Entity<Register>(entity =>
        {
            entity.HasKey(e => e.IdRegister).HasName("Registers_pkey");

            entity.ToTable("Registers", "BeClever");

            entity.Property(e => e.IdRegister).HasColumnName("idRegister");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.IdBusiness).HasColumnName("idBusiness");
            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");
            entity.Property(e => e.RegisterType)
                .HasMaxLength(20)
                .HasColumnName("register_type");

            entity.HasOne(d => d.IdBusinessNavigation).WithMany(p => p.Registers)
                .HasForeignKey(d => d.IdBusiness)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BUSINESS");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Registers)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMPLOYEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
