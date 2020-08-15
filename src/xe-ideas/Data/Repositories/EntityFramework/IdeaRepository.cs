using Microsoft.EntityFrameworkCore;
using System.Linq;
using xe_ideas.Data.Repositories.Interfaces;
using xe_ideas.Models;
using xe_ideas.Models.LookUp;

namespace xe_ideas.Data.Repositories.EntityFramework
{
    public class IdeaRepository : IIdeaRepository
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<Idea> DbSet;

        public IdeaRepository(ApplicationDbContext context)
        { 
            this.Context = context;
            this.DbSet = this.Context.Set<Idea>();
        }

        public int Create(Idea item)
        {
            this.Context.Add(item);
            this.Context.SaveChanges();

            return item.Id;
        }

        public void Delete(Idea item)
        {
            this.Context.Remove(item);
            this.Context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            this.Context.Remove(new Idea() { Id = id });
            this.Context.SaveChanges();
        }

        public void Update(Idea item)
        {
            var entity = this.Context.Find(item.GetType(), item.Id);

            this.Context.Entry(entity).CurrentValues.SetValues(item);
            this.Context.SaveChanges();
        }

        public IQueryable<Idea> GetAllPublic()
        {
            return this.DbSet
                .Where(x => x.PrivacyId == IdeaPrivacy.Public.Id);
        }

        public Idea GetById(int id)
        {
            return this.DbSet
                .SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Idea> GetByCreatorId(string creatorId)
        {
            return this.DbSet
                .Where(x => x.CreatorId == creatorId);
        }
    }
}
