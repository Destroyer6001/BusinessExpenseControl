using BusinessExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessExpenseControl.Infraestructure.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
            
        }

        public DbSet<CompanyEntity> Company => Set<CompanyEntity>();

        public DbSet<CategoryEntity> Category => Set<CategoryEntity>();

        public DbSet<UserEntity> User => Set<UserEntity>();
        
        public DbSet<JourneyEntity> Journey => Set<JourneyEntity>();

        public DbSet<JourneyLogsEntity> JourneyLog => Set<JourneyLogsEntity>();

        public DbSet<CompanyUserEntity> CompanyUser => Set<CompanyUserEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyUserEntity>()
                .HasOne(j => j.Company)
                .WithMany(p => p.CompanyUsers)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyUserEntity>()
                .HasOne(j => j.User)
                .WithMany(p => p.CompanyUsers)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JourneyEntity>()
                .HasOne(j => j.Creator)
                .WithMany(u => u.JourneysCreate)
                .HasForeignKey(j => j.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JourneyEntity>()
                .HasOne(j => j.Responsible)
                .WithMany(u => u.JourneysAsigned)
                .HasForeignKey(j => j.ResponsibleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JourneyEntity>()
                .HasOne(j => j.Company)
                .WithMany(c => c.Journeys)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JourneyLogsEntity>()
                .HasOne(j => j.Journey)
                .WithMany(ju => ju.JourneyLogs)
                .HasForeignKey(j => j.JourneyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JourneyLogsEntity>()
                .HasOne(j => j.Category)
                .WithMany(c => c.JourneyLogs)
                .HasForeignKey(j => j.JourneyId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }


    }
}
