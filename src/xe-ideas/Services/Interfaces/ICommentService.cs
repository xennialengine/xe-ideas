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
        IEnumerable<Comment> GetByCreatorId(ApplicationContext context, string creatorId, int skip = 0, int take = 50);
        IEnumerable<Comment> GetByCreatorUsername(ApplicationContext context, string username, int skip = 0, int take = 50);
        IEnumerable<Comment> GetByIdeaId(ApplicationContext context, int ideaId, int skip = 0, int take = 50);
        
    }
}
