using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IIdeaService ideaService;

        public IdeaController(ILogger<WeatherForecastController> logger, IIdeaService ideaService)
        {
            this._logger = logger;
            this.ideaService = ideaService;
        }

        [HttpGet]
        public IEnumerable<Idea> Get(string username)
        {
            ApplicationContext context = new ApplicationContext()
            {

            };

            return this.ideaService.GetByCreatorId(context, "6761d1ea-06bb-4c3e-b24e-8a7865bf094b");
        }
    }
}
