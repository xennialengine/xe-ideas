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
    public class CommentController : ControllerBase
    {
        public ApplicationContext Context;
        private readonly IApplicationUserService applicationUserService;
        private readonly ICommentService commentService;

        public CommentController(IApplicationUserService applicationUserService , ICommentService commentService)
        {
            this.applicationUserService = applicationUserService;
            this.commentService = commentService;
        }        

        [HttpPost]
        [Route("api/idea/{ideaId}/comment/")]
        public Comment Update(Comment comment)
        {
            this.Context = this.CreateApplicationContext((ClaimsIdentity)HttpContext.User.Identity);

            comment.CreatorId = this.Context.CurrentUser.Id;
            comment.Content = comment.Content;
            comment.CreatedDate = DateTime.UtcNow;
            comment.LastModifiedDate = DateTime.UtcNow;
            
            comment.Id = this.commentService.Create(this.Context, comment);
            
            comment.Creator = new ApplicationUser 
            {
                Id = this.Context.CurrentUser.Id,
                UserName = this.Context.CurrentUser.UserName
            };

            return comment;
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
