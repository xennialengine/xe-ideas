using Microsoft.EntityFrameworkCore;
using System.Linq;
using xe_ideas.Data.Repositories.Interfaces;
using xe_ideas.Models;

namespace xe_ideas.Data.Repositories.EntityFramework
{
    public class CommentRepository : ICommentRepository
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<Comment> DbSet;

        public CommentRepository(ApplicationDbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<Comment>();
         }

        public int Create(Comment item)
        {
            this.Context.Add(item);
            this.Context.SaveChanges();

            return item.Id;
        }

        public void Delete(Comment item)
        {
            this.Context.Remove(item);
            this.Context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            this.Context.Remove(new Comment() { Id = id });
            this.Context.SaveChanges();
        }

        public void Update(Comment item)
        {
            var entity = this.Context.Find(item.GetType(), item.Id);

            this.Context.Entry(entity).CurrentValues.SetValues(item);
            this.Context.SaveChanges();
        }

        public Comment GetById(int id)
        {
            return this.DbSet
                .SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Comment> GetByCreatorId(string creatorId)
        {
            return this.DbSet
                .Where(x => x.CreatorId == creatorId);
        }

        public IQueryable<Comment> GetByIdeaId(int ideaId)
        {
            return this.DbSet
                .Where(x => x.IdeaId == ideaId);
        }
    }
}
