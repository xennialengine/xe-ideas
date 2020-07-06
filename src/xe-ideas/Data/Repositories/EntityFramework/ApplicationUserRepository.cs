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
            this.DbSet = this.Context.Set<IdentityUser>();
        }

        public IQueryable<IdentityUser> GetByUsername(string username)
        {
            return this.DbSet.Where(x => x.UserName.ToUpper() == username.ToUpper());
        }
    }
}
