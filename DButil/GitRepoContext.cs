using GitRepositoryTracker.Data;
using GitRepositoryTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GitRepositoryTracker.DButil
{
    public class GitRepoContext : IdentityUserContext<IdentityUser>
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<RepositoryTopic> RepositoryTopics { get; set; }


        public GitRepoContext(DbContextOptions<GitRepoContext> options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RepositoryTopic>()
                .HasKey(rt => new { rt.RepositoryId, rt.TopicId });

            modelBuilder.Entity<RepositoryTopic>()
                .HasOne<Repository>(rt => rt.Repository)
                .WithMany(r => r.RepositoryTopics)
                .HasForeignKey(rt => rt.RepositoryId);

            modelBuilder.Entity<RepositoryTopic>()
                .HasOne<Topic>(rt => rt.Topic)
                .WithMany(r => r.RepositoryTopics)
                .HasForeignKey(rt => rt.TopicId);

            modelBuilder.Entity<Repository>()
                .HasIndex(r => r.Url)
                .IsUnique();

            modelBuilder.Entity<Topic>()
                .Property(t => t.TopicId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Topic>()
                .HasIndex(t => t.TopicName)
                .IsUnique();
            modelBuilder.Entity<Repository>()
                .HasOne(r => r.Language)
                .WithMany(l => l.Repositories)
                .HasForeignKey(r => r.LanguageId);
            modelBuilder.Entity<Language>()
                .Property(l => l.LanguageId)
                .ValueGeneratedOnAdd();

        }

    }


}
