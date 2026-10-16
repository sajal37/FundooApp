
using RepositoryLayer.Entity;
using ModelLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(
            DbContextOptions<FundooContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(user => user.Email)
                .IsUnique();
        }
    }
}