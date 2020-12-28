using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using xe_ideas.Data.Repositories.Interfaces;

namespace xe_ideas.Data.Repositories.EntityFramework
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<IdentityUser> DbSet;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }

        public IQueryable<IdentityUser> GetByUserId(string userId)
        {
            return (string.IsNullOrWhiteSpace(userId))
                ? Enumerable.Empty<IdentityUser>().AsQueryable()
                : this.Context.Users.Where(x => x.Id.ToUpper() == userId.ToUpper());
        }

        public IQueryable<IdentityUser> GetByUsername(string username)
        {
            return (string.IsNullOrWhiteSpace(username))
                ? Enumerable.Empty<IdentityUser>().AsQueryable()
                : this.Context.Users.Where(x => x.UserName.ToUpper() == username.ToUpper());
        }
    }
}
