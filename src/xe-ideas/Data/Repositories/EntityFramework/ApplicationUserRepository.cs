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

        public IQueryable<IdentityUser> GetByUsername(string username)
        {
            return this.Context.Users.Where(x => x.UserName.ToUpper() == username.ToUpper());
        }
    }
}
