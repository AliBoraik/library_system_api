using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Subjects)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmentId);

        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Lectures)
            .WithOne(l => l.Subject)
            .HasForeignKey(l => l.SubjectId);

        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Books)
            .WithOne(b => b.Subject)
            .HasForeignKey(b => b.SubjectId);

        modelBuilder.Entity<Lecture>()
            .HasOne(l => l.User)
            .WithMany(u => u.Lectures)
            .HasForeignKey(l => l.UploadedBy);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.UploadedBy);

        // Seed initial data
        var department1 = new Department
        {
            DepartmentId = Guid.NewGuid(),
            Name = "Computer Science",
            Description = "Department of Computer Science"
        };

        var department2 = new Department
        {
            DepartmentId = Guid.NewGuid(),
            Name = "Mathematics",
            Description = "Department of Mathematics"
        };
        modelBuilder.Entity<Department>().HasData(department1, department2);
        modelBuilder.Entity<Subject>().HasData(
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                DepartmentId = department1.DepartmentId,
                Name = "Algorithms",
                Description = "Study of algorithms"
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                DepartmentId = department1.DepartmentId,
                Name = "Data Structures",
                Description = "Study of data structures"
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                DepartmentId = department2.DepartmentId,
                Name = "Calculus",
                Description = "Study of calculus"
            },
            new Subject
            {
                SubjectId = Guid.NewGuid(),
                DepartmentId = department2.DepartmentId,
                Name = "Linear Algebra",
                Description = "Study of linear algebra"
            }
        );
    }
}