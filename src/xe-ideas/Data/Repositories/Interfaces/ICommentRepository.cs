using xe_ideas.Models;
using System.Linq;

namespace xe_ideas.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        int Create(Comment item);
        void Delete(Comment item);
        void DeleteById(int id);
        Comment GetById(int id);
        void Update(Comment item);
        IQueryable<Comment> GetByCreatorId(string creatorId);
        IQueryable<Comment> GetByIdeaId(int ideaId);
    }
}
