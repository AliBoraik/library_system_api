using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataContext
{
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
        }
    }
}