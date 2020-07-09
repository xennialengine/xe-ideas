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
        IEnumerable<Idea> GetByCreatorId(ApplicationContext context, string creatorId);
        IEnumerable<Idea> GetByCreatorUsername(ApplicationContext context, string username);
    }
}
