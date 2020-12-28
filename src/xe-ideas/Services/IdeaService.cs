using xe_ideas.Data.Repositories.Interfaces;
using xe_ideas.Models;
using xe_ideas.Models.LookUp;
using xe_ideas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security;
using Microsoft.Data.SqlClient;

namespace xe_ideas.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository ideaRepository;
        private readonly IApplicationUserRepository applicationUserRepository;

        public IdeaService(IIdeaRepository ideaRepository, IApplicationUserRepository identityUserRepository)
        {
            this.ideaRepository = ideaRepository;
            this.applicationUserRepository = identityUserRepository;
        }

        public int Create(ApplicationContext context, Idea item)
        {
            return this.ideaRepository.Create(item);
        }

        /// <summary>
        /// Delete an Idea by id.
        /// 
        /// PERMISSIONS
        ///   Only allow Idea creator to delete it.
        ///   TODO allow admin in the future?
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        public void DeleteById(ApplicationContext context, int id)
        {
            // If there's no current user, don't allow
            if (context.CurrentUser == null)
            {
                throw new SecurityException($"User NULL is unauthorized to delete Idea id {id}");
            }

            var item = this.ideaRepository.GetById(id);
            
            // If the item doesn't exist, treat it as if the item has been deleted
            if (item == null)
            {
                return;
            }

            // TODO allow Admin 
            // If the current user is not the item creator, don't allow
            if (item.CreatorId != context.CurrentUser.Id)
            {
                throw new SecurityException($"User {context.CurrentUser.Id} is unauthorized to delete Idea id {id}");
            }
            
            try
            {
                this.ideaRepository.Delete(item);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // This exception is expected when trying to delete an item that does not exist.
                // We catch it and let it through, because the end result is as intially requested: the item no longer exists.
                // For any other exception, we let it bubble up.
                if (!ex.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)"))
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Returns an Idea by id.
        /// 
        /// PERMISSIONS
        ///   Allow anybody to see all public Activities.
        ///   Only allow Idea creator to see private Activities.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Idea GetById(ApplicationContext context, int id)
        {
            var item = this.ideaRepository.GetById(id);

            if (item != null)
            {
                if (item.PrivacyId == IdeaPrivacy.Private.Id
                    && item.CreatorId != context.CurrentUser.Id)
                {
                    throw new SecurityException($"User {context.CurrentUser.Id} is unauthorized to view Idea id {id}");
                }
            }

            return item;
        }

        public IEnumerable<Idea> GetAllPublic(ApplicationContext context, int skip = 0, int take = 50)
        {
            return this.ideaRepository
                       .GetAllPublic()
                       .Skip(skip)
                       .Take(take)
                       .Include(x => x.Creator);
        }

        public IEnumerable<Idea> GetByCreatorId(ApplicationContext context, string creatorId, int skip = 0, int take = 50)
        {
            return this.ideaRepository
                       .GetByCreatorId(creatorId)
                       .Skip(skip)
                       .Take(take);
        }

        public IEnumerable<Idea> GetByCreatorUsername(ApplicationContext context, string username, int skip = 0, int take = 50)
        {
            var user = this.applicationUserRepository.GetByUsername(username).FirstOrDefault();

            if (user != null) 
            {
                var list = this.ideaRepository
                               .GetByCreatorId(user.Id)
                               .Skip(skip)
                               .Take(take);

                return (user.Id == context.CurrentUser.Id)
                    ? list
                    : list.Where(x => x.PrivacyId == IdeaPrivacy.Public.Id);
            }

            return Enumerable.Empty<Idea>();
        }

        public void Update(ApplicationContext context, Idea item)
        {
            // If there's no current user, don't allow
            if (context.CurrentUser == null)
            {
                throw new SecurityException($"User NULL is unauthorized to delete Idea id {item.Id}");
            }

            var existingItem = this.ideaRepository.GetById(item.Id);
            
            // If the item doesn't exist, throw an exception
            if (existingItem == null)
            {
                throw new NullReferenceException($"Idea id {item.Id} does not exist.");
            }

            // TODO allow Admin 
            // If the current user is not the item creator, don't allow
            if (existingItem.CreatorId != context.CurrentUser.Id)
            {
                throw new SecurityException($"User {context.CurrentUser.Id} is unauthorized to update Idea id {item.Id}.");
            }

            // TODO make this work with PATCH
            existingItem.Name = item.Name;
            existingItem.PrivacyId = item.PrivacyId;
            existingItem.Description = item.Description;
            existingItem.LastModifiedDate = DateTime.UtcNow;

            try
            {
                this.ideaRepository.Update(existingItem);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is SqlException && ex.InnerException.Message.Contains("IX_Ideas_CreatorId_Name"))
                    {
                        throw new ArgumentException("Name already exists for this account.  Please choose another one.");
                    }
                }

                throw ex;
            }
        }
    }
}
