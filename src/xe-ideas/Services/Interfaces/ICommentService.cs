using System.Collections.Generic;
using xe_ideas.Models;

namespace xe_ideas.Services.Interfaces
{
    public interface ICommentService
    {
        int Create(ApplicationContext context, Comment item);
        void DeleteById(ApplicationContext context, int id);
        Comment GetById(ApplicationContext context, int id);
        void Update(ApplicationContext context, Comment item);
        IEnumerable<Comment> GetByIdeaId(ApplicationContext context, int ideaId);
        IEnumerable<Comment> GetByCreatorId(ApplicationContext context, string creatorId);
        IEnumerable<Comment> GetByCreatorUsername(ApplicationContext context, string username);
        
    }
}
