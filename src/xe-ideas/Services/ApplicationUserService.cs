using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security;
using xe_ideas.Data.Repositories.Interfaces;
using xe_ideas.Models;
using xe_ideas.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace xe_ideas.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository applicationUserRepository;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
        }

        public ApplicationUser GetByUserId(ApplicationContext context, string userId)
        {
            return this.applicationUserRepository.GetByUserId(userId).FirstOrDefault() as ApplicationUser;
        }

        public ApplicationUser GetByUsername(ApplicationContext context, string username)
        {
            return this.applicationUserRepository.GetByUsername(username).FirstOrDefault() as ApplicationUser;
        }
    }
}
