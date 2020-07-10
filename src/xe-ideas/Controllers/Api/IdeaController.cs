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
        [Route("api/idea/{ideaId}")]
        public Idea GetById(int ideaId)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            var idea = this.ideaService.GetById(this.Context, ideaId);

            // TODO check to make sure the user has permission to view this
            
            return idea;
        }

        [HttpGet]
        [Route("api/idea")]
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
