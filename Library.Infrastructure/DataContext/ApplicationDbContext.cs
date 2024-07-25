using Library.Domain.Constants;
using Library.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataContext;
//  dotnet ef --startup-project ../Library.Api/ migrations add Initial
//  dotnet ef --startup-project ../Library.Api/ database update

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
        
        // roles ids
        var adminRoleId = Guid.NewGuid().ToString();
        var teacherRoleId = Guid.NewGuid().ToString();
        // admin id 
        var adminId = Guid.NewGuid().ToString();
        
        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpper()
            });
        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = teacherRoleId,
                Name = AppRoles.Teacher,
                NormalizedName = AppRoles.Teacher.ToUpper()
            });
        modelBuilder.Entity<ApplicationUser>()
            .HasData(new ApplicationUser
            {
                Id = adminId,
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q=="
            });
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminId
        });
        
        
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