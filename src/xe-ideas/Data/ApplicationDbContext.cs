using xe_ideas.Models;
using xe_ideas.Models.LookUp;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xe_ideas.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<IdeaPrivacy> IdeaPrivacies { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make sure to call IdentityDbContext.OnModelCreating() so it can do its thing
            base.OnModelCreating(modelBuilder);

            #region Data tables
            modelBuilder.Entity<Idea>().HasIndex(x => new { x.CreatorId, x.Name }).IsUnique();
            modelBuilder.Entity<Idea>().Property(x => x.CreatorId).IsRequired();
            modelBuilder.Entity<Idea>().Property(x => x.IsActive).HasDefaultValue(true); // C# property is also defaulted to true
            modelBuilder.Entity<Idea>().Property(x => x.PrivacyId).HasDefaultValue(1);
            modelBuilder.Entity<Idea>().Property(x => x.Name).IsRequired();
            //modelBuilder.Entity<Idea>().Property(x => x.Problem);
            //modelBuilder.Entity<Idea>().Property(x => x.Solution);
            //modelBuilder.Entity<Idea>().Property(x => x.Description);
            modelBuilder.Entity<Idea>().Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Idea>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getutcdate()");
            //modelBuilder.Entity<Idea>().Property(x => x.LastCompletedDate);

            modelBuilder.Entity<Comment>().Property(x => x.IdeaId).HasDefaultValue(1);
            modelBuilder.Entity<Comment>().Property(x => x.CreatorId).IsRequired();
            //modelBuilder.Entity<Comment>().Property(x => x.Content).IsRequired();
            modelBuilder.Entity<Comment>().Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Comment>().Property(x => x.LastModifiedDate).HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<ApplicationUser>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<ApplicationUser>().Property(x => x.UserName).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(x => x.IsActive).HasDefaultValue(true); // C# property is also defaulted to true
            modelBuilder.Entity<ApplicationUser>().Property(x => x.Email).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
            #endregion

            #region LookUp tables
            modelBuilder.Entity<IdeaPrivacy>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<IdeaPrivacy>().Property(x => x.Id).ValueGeneratedNever(); // LookUp tables do not have identity Ids
            #endregion

            #region Disable cascade delete
            // This is a workaround since EF 2.0 does not have a global setting to disable cascade delete.
            var cascadeFks = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(fk => fk.IsOwnership == false && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFks)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            #region Seeding look up values.  Required by the application.
            modelBuilder.Entity<IdeaPrivacy>().HasData(new IdeaPrivacy { Id = 1, Name = "Private", Description = "Can be viewed only by owner" });
            modelBuilder.Entity<IdeaPrivacy>().HasData(new IdeaPrivacy { Id = 2, Name = "Public", Description = "Can be viewed by anybody" });
            #endregion

            #region Seeding test values
            var userId1 = "6761d1ea-06bb-4c3e-b24e-8a7865bf094b";
            var userId2 = "00000000-0000-0000-0000-000000000002";
            var userId3 = "00000000-0000-0000-0000-000000000003";

            // Password = Password1$
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser {
                Id = userId1,
                UserName = "superkarn@gmail.com",
                NormalizedUserName = "SUPERKARN@GMAIL.COM",
                Email = "superkarn@gmail.com",
                NormalizedEmail = "SUPERKARN@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEKgE7GuPx6Xp3+6/itEA+GIYEVnxdMKCDMyuPFeXlH1sZiH1lZ+S2QO2fE2JYxOxpQ==",
                SecurityStamp = "BF6DFMXSJX3USJURBYD3EWOANZ4SUDDL",
                ConcurrencyStamp = "a9f508b7-ab66-4b2f-8d57-b73fd1ac898b"                
            });
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = userId2, UserName = "test", Email = "test@example.com" });
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = userId3, UserName = "test2", Email = "test2@example.com" });

            modelBuilder.Entity<Idea>().HasData(new Idea { Id = 1, CreatorId = userId1, Name = "Test Idea", Description = "Test Idea..." });
            modelBuilder.Entity<Idea>().HasData(new Idea { Id = 2, CreatorId = userId1, Name = "Test 2", Description = "blah", PrivacyId = 2 });
            modelBuilder.Entity<Idea>().HasData(new Idea { Id = 3, CreatorId = userId1, Name = "Test 3", Description = "blah blah blah" });
            modelBuilder.Entity<Idea>().HasData(new Idea { Id = 4, CreatorId = userId2, Name = "Test 4" });
            modelBuilder.Entity<Idea>().HasData(new Idea { Id = 5, CreatorId = userId2, Name = "Test 5", PrivacyId = 2 });

            modelBuilder.Entity<Comment>().HasData(new Comment { Id = 1, IdeaId = 1, CreatorId = userId1, Content = "Test comment" });
            #endregion
        }
    }
}
