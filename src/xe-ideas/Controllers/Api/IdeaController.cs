using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using xe_ideas.Models;
using xe_ideas.Services.Interfaces;

namespace xe_ideas.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdeaController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        public ApplicationContext Context;
        private readonly IIdeaService ideaService;

        public IdeaController(ILogger<WeatherForecastController> logger, IIdeaService ideaService)
        {
            this._logger = logger;
            this.ideaService = ideaService;
        }

        [HttpGet]
        public IEnumerable<Idea> Get(string username)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            return string.IsNullOrWhiteSpace(username)
                ? this.ideaService.GetAllPublic(this.Context)
                : this.ideaService.GetByCreatorUsername(this.Context, username);
        }
        

        protected ApplicationContext CreateApplicationContext(ClaimsIdentity identity)
        {
            return new ApplicationContext()
            {
                CurrentUser = new ApplicationUser()
                {
                    // TODO add userId and username to JWT payload
                    //Id = identity.Claims.FirstOrDefault(x => x.Type == "userId").Value,
                    //UserName = identity.Claims.FirstOrDefault(x => x.Type == "username").Value
                }
            };
        }
    }
}
