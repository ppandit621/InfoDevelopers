using InfoDevelopers.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InfoDevelopers.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeQualification> EmployeeeQualifications { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeQualification>()
                .HasKey(eq => new { eq.EmployeeId, eq.QualificationId });

            modelBuilder.Entity<EmployeeQualification>()
                .HasOne(eq => eq.Employee)
                .WithMany(e => e.EmployeeQualifications)
                .HasForeignKey(eq => eq.EmployeeId);

            modelBuilder.Entity<EmployeeQualification>()
                .HasOne(eq => eq.Qualification)
                .WithMany()
                .HasForeignKey(eq => eq.QualificationId);
        }
    }
}
