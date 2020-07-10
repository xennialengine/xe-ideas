using System.Collections.Generic;
using xe_ideas.Models;

namespace xe_ideas.Services.Interfaces
{
    public interface IIdeaService
    {
        int Create(ApplicationContext context, Idea item);
        void DeleteById(ApplicationContext context, int id);
        Idea GetById(ApplicationContext context, int id);
        void Update(ApplicationContext context, Idea item);
        IEnumerable<Idea> GetAllPublic(ApplicationContext context, int skip = 0, int take = 50);
        IEnumerable<Idea> GetByCreatorId(ApplicationContext context, string creatorId, int skip = 0, int take = 50);
        IEnumerable<Idea> GetByCreatorUsername(ApplicationContext context, string username, int skip = 0, int take = 50);
    }
}
