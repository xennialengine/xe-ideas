using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace xe_ideas.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        IQueryable<IdentityUser> GetByUsername(string username);
    }
}
