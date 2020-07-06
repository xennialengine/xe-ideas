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
    public class LookUpController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILookUpService lookUpService;

        public LookUpController(ILogger<WeatherForecastController> logger, ILookUpService lookUpService)
        {
            this._logger = logger;
            this.lookUpService = lookUpService;
        }

        [HttpGet]
        public ActionResult<IDictionary<string, IEnumerable<BaseLookUpModel>>> Get(string username)
        {
            ApplicationContext context = new ApplicationContext()
            {

            };

            return new ActionResult<IDictionary<string, IEnumerable<BaseLookUpModel>>>(this.lookUpService.GetAll(context));
        }
    }
}
