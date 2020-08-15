using xe_ideas.Models;
using System.Linq;

namespace xe_ideas.Data.Repositories.Interfaces
{
    public interface IIdeaRepository
    {
        int Create(Idea item);
        void Delete(Idea item);
        void DeleteById(int id);
        Idea GetById(int id);
        void Update(Idea item);
        IQueryable<Idea> GetAllPublic();
        IQueryable<Idea> GetByCreatorId(string creatorId);
    }
}
