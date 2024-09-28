using Library.Domain.Constants;
using Library.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataContext;
//  dotnet ef --startup-project ../Library.Api/ migrations add Initial
//  dotnet ef --startup-project ../Library.Api/ database update

public class ApplicationDbContext : IdentityDbContext<User>
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
        
        modelBuilder.Entity<Teacher>()
            .HasMany(t => t.Subjects)
            .WithOne(s => s.Teacher)
            .HasForeignKey(s => s.TeacherId);
        
        modelBuilder.Entity<User>()
            .HasDiscriminator<UserType>("UserType")
            .HasValue<User>(UserType.Admin)
            .HasValue<Teacher>(UserType.Teacher)
            .HasValue<Student>(UserType.Student);

        // roles ids
        var adminRoleId = Guid.NewGuid().ToString();
        var teacherRoleId = Guid.NewGuid().ToString();
        var studentRoleId = Guid.NewGuid().ToString();
        // admin id 
        var adminId = Guid.NewGuid().ToString();
        // teacher id
        var teacherId = Guid.NewGuid().ToString();
        // student id
        var studentId = Guid.NewGuid().ToString();
        
        
        
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
        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = studentRoleId,
                Name = AppRoles.Student,
                NormalizedName = AppRoles.Student.ToUpper()
            });
        
        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = adminId,
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q=="
            });
        modelBuilder.Entity<Teacher>()
            .HasData(new Teacher
            {
                Id = teacherId,
                Email = "teacher@gmail.com",
                NormalizedEmail = "TEACHER@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "teacher",
                NormalizedUserName = "TEACHER",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
                
            });
        
        modelBuilder.Entity<Student>()
            .HasData(new Student
            {
                Id = studentId,
                Email = "student@gmail.com",
                NormalizedEmail = "STUDENT@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student",
                NormalizedUserName = "STUDENT",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q=="
            });
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminId
            });
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = teacherRoleId,
                UserId = teacherId
            });
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = studentRoleId,
                UserId = studentId
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
                Name = "Message Structures",
                Description = "Study of data structures",
                TeacherId = teacherId
            }
        );
    }
}