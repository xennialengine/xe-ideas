using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xe_ideas.Models;
using xe_ideas.Services.Interfaces;

namespace xe_ideas.Controllers.Api
{
    [Authorize]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        public ApplicationContext Context;
        private readonly IIdeaService ideaService;
        private readonly ICommentService commentService;

        public IdeaController(IIdeaService ideaService, ICommentService commentService)
        {
            this.ideaService = ideaService;
            this.commentService = commentService;
        }

        [HttpGet]
        [Route("api/idea/{ideaId}")]
        public Idea GetById(int ideaId)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            var idea = this.ideaService.GetById(this.Context, ideaId);

            // TODO check to make sure the user has permission to view this
            
            // Get associated comments
            idea.Comments = this.commentService.GetByIdeaId(this.Context, ideaId).ToList();

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

        

        [HttpPut]
        [Route("api/idea/{ideaId}")]
        public void Update(string username, Idea idea)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            this.ideaService.Update(this.Context, idea);
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
