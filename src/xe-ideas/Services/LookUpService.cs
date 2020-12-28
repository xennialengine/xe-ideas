using xe_ideas.Models;
using xe_ideas.Models.LookUp;
using xe_ideas.Services.Interfaces;
using System.Collections.Generic;

namespace xe_ideas.Services.LookUp
{
    public class LookUpService : ILookUpService
    {
        public LookUpService()
        { }

        public IDictionary<string, IEnumerable<BaseLookUpModel>> GetAll(ApplicationContext context)
        {
            return new Dictionary<string, IEnumerable<BaseLookUpModel>>
            {
                { "IdeaPrivacies", IdeaPrivacy.GetValues() },
            };
        }
    }
}
