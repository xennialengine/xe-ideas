using Microsoft.AspNetCore.Identity;
using xe_ideas.Models;

namespace xe_ideas.Services.Interfaces
{
    public interface IApplicationUserService
    {
        ApplicationUser GetByUserId(ApplicationContext context, string userId);
        ApplicationUser GetByUsername(ApplicationContext context, string username);
    }
}
