using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xe_ideas.Models;
using xe_ideas.Models.LookUp;
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
        public ActionResult<Idea> GetById(int ideaId)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            try 
            {
                var idea = this.ideaService.GetById(this.Context, ideaId);
                
                // Get associated comments
                idea.Comments = this.commentService.GetByIdeaId(this.Context, ideaId).ToList();

                return idea;
            }
            catch(SecurityException)
            {
                // If the user is not authorized to view this, return 404 (not found)
                return StatusCode(404);
            }
            catch(Exception)
            {
                // Something else happened, return 500 (server error)
                return StatusCode(500);
            }
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
        public ActionResult Update(string username, Idea idea)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);
            
            try 
            {
                this.ideaService.Update(this.Context, idea);

                return StatusCode(200);
            }
            catch(SecurityException)
            {
                // The user is not authorized to do this
                return StatusCode(401);
            }
            catch(Exception)
            {
                // Something else happened, return 500 (server error)
                return StatusCode(500);
            }
        }
        

        protected ApplicationContext CreateApplicationContext(ClaimsIdentity identity)
        {
            return new ApplicationContext()
            {
                CurrentUser = new ApplicationUser()
                {
                    Id = identity.Claims.FirstOrDefault(x => x.Type == "userId").Value,
                    UserName = identity.Claims.FirstOrDefault(x => x.Type == "userName").Value
                }
            };
        }
    }
}
