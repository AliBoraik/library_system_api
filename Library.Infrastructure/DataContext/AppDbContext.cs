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

        modelBuilder.Entity<NotificationModel>(entity =>
        {
            entity.HasKey(n => n.NotificationId); // Set primary key explicitly

            entity.HasOne(n => n.RecipientUser) // Configure recipient relationship
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            entity.HasOne(n => n.SenderUser) // Configure sender relationship
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        });


        // roles ids
        var adminRoleId = Guid.NewGuid();
        var teacherRoleId = Guid.NewGuid();
        var studentRoleId = Guid.NewGuid();
        // admin id 
        var adminId = Guid.NewGuid();
        // user 1 id
        var user1Id = Guid.NewGuid();
        // user 2 id
        var user2Id = Guid.NewGuid();


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

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminId
            });

        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = user1Id,
                Email = "teacher@gmail.com",
                NormalizedEmail = "TEACHER@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "teacher",
                NormalizedUserName = "TEACHER",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q=="
            });

        modelBuilder.Entity<Teacher>()
            .HasData(new Teacher
            {
                TeacherId = Guid.NewGuid(),
                UserId = user1Id
            });

        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = user2Id,
                Email = "student@gmail.com",
                NormalizedEmail = "STUDENT@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "student",
                NormalizedUserName = "STUDENT",
                PasswordHash = "AQAAAAIAAYagAAAAEB06+sY86pJ8aS/cc9CPo9ut/NBhGXU6rZO/YXvY33qmZqz2L97P27e13UvDnGx+7Q=="
            });

        modelBuilder.Entity<Student>()
            .HasData(new Student
            {
                StudentId = Guid.NewGuid(),
                UserId = user2Id
            });


        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = teacherRoleId,
                UserId = user1Id
            });

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = studentRoleId,
                UserId = user2Id
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
    }
}