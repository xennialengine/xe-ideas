using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using xe_ideas.Models;

namespace xe_ideas.Services
{
    /// Got this class from https://stackoverflow.com/a/44795605/1398750
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await this.userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id),
                new Claim("userName", user.UserName),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = await this.userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.IsActive;
        }
    }
}