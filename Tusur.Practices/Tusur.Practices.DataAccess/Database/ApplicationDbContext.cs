using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Persistence.Database.Entities;

namespace Tusur.Practices.Persistence.Database
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Defaults

            builder.Entity<Notification>()
                .Property(p => p.SentAt)
                .HasDefaultValueSql("now()::timestamp");

            builder.Entity<Proxy>()
                .Property(p => p.ValidFrom)
                .HasDefaultValueSql("now()::timestamp");

            builder.Entity<Entities.Application>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("now()::timestamp");

            builder.Entity<Comment>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("now()::timestamp");

            builder.Entity<IdentityRole<Guid>>().HasData(DatabaseDefaults.GetIdentityRoles());

            // Delete

            // Create

            base.OnModelCreating(builder);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<DepartmentHead> DepartmentHeads { get; set; }

        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<StudyField> StudyFields { get; set; }
        public DbSet<StudyForm> StudyForms { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }

        public DbSet<ApprovedStudyPlan> ApprovedStudyPlans { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Proxy> Proxies { get; set; }
        public DbSet<Signatory> Signatories { get; set; }

        public DbSet<PracticeKind> PracticeKinds { get; set; }
        public DbSet<PracticeType> PracticeTypes { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<PracticeDate> PracticeDates { get; set; }
        public DbSet<ApprovedPractice> ApprovedPractices { get; set; }

        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Entities.Application> Applications { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<PracticeProfile> PracticeProfiles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractContent> ContractContents { get; set; }
        public DbSet<Organization> Organizations { get; set; }
    }
}
