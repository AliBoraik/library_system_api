using Library.Domain.Constants;
using Library.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataContext;
//  dotnet ef --startup-project ../Library.Api/ migrations add Initial
//  dotnet ef --startup-project ../Library.Api/ database update

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; init; }
    public DbSet<Subject> Subjects { get; init; }
    public DbSet<Lecture> Lectures { get; init; }
    public DbSet<Book> Books { get; init; }

    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<NotificationModel> Notifications { get; init; }

    public DbSet<UserNotification> UserNotifications { get; init; }

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

        // One-to-Many configuration
        modelBuilder.Entity<User>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(s => s.DepartmentId);

        modelBuilder.Entity<UserNotification>()
            .HasKey(un => new { un.NotificationId, un.UserId }); // Composite key

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.Notification)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(un => un.NotificationId);

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.User)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(un => un.UserId);

        // roles ids
        var adminRoleId = Guid.NewGuid();
        var teacherRoleId = Guid.NewGuid();
        var studentRoleId = Guid.NewGuid();
        // admin id 
        var adminId = AdminConstants.SystemAdminId;
        // teacherId
        var teacherId = Guid.Parse("f33f8675-06a1-4a28-b111-f7201cd6eb2f");
        // studentId
        var studentId = Guid.Parse("bdbb81d1-824f-41c7-b4a9-982c8dcb13dc");
        // passwordHasher 
        var passwordHasher = new PasswordHasher<User>();

        modelBuilder.Entity<IdentityRole<Guid>>()
            .HasData(new IdentityRole<Guid>
            {
                Id = adminRoleId,
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpper()
            });
        modelBuilder.Entity<IdentityRole<Guid>>()
            .HasData(new IdentityRole<Guid>
            {
                Id = teacherRoleId,
                Name = AppRoles.Teacher,
                NormalizedName = AppRoles.Teacher.ToUpper()
            });
        modelBuilder.Entity<IdentityRole<Guid>>()
            .HasData(new IdentityRole<Guid>
            {
                Id = studentRoleId,
                Name = AppRoles.Student,
                NormalizedName = AppRoles.Student.ToUpper()
            });
        
        // Seed admin user
        var adminUser = new User
        {
            Id = AdminConstants.SystemAdminId,
            UserName = AdminConstants.SystemAdminUserName,
            NormalizedUserName = AdminConstants.SystemAdminUserName.ToUpper(),
            Email = AdminConstants.SystemAdminEmail,
            NormalizedEmail = AdminConstants.SystemAdminEmail.ToUpper(),
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, AdminConstants.DefaultAdminPassword)
        };

        // Seed teacher user
        var teacherSeed = new User
        {
            Id = teacherId,
            Email = "teacher@system.com",
            NormalizedEmail = "TEACHER@SYSTEM.COM",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "teacher",
            NormalizedUserName = "TEACHER",
            PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
            DepartmentId = 1
        };

        // Seed student user
        var studentSeed = new User
        {
            Id = studentId,
            Email = "student@system.com",
            NormalizedEmail = "STUDENT@SYSTEM.COM",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "student",
            NormalizedUserName = "STUDENT",
            PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q==",
            DepartmentId = 1
        };

        modelBuilder.Entity<User>()
            .HasData(adminUser , teacherSeed , studentSeed);
        
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminId
            });
        
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = teacherRoleId,
                UserId = teacherId
            });

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = studentRoleId,
                UserId = studentId
            });


        modelBuilder.Entity<Teacher>()
            .HasData(new Teacher
            {
                Id = teacherId
            });
        
        modelBuilder.Entity<Student>()
            .HasData(new Student
            {
                Id = studentId
            });

        var department1 = new Department
        {
            Id = 1,
            Name = "Computer Science",
            Description = "Department of Computer Science"
        };

        var department2 = new Department
        {
            Id = 2,
            Name = "Mathematics",
            Description = "Department of Mathematics"
        };
        modelBuilder.Entity<Department>().HasData(department1, department2);
    }
}