using System.Collections.Generic;

namespace xe_ideas.Models.LookUp
{
    public class IdeaPrivacy : BaseLookUpModel
    {
        public static IEnumerable<IdeaPrivacy> GetValues()
        {
            yield return IdeaPrivacy.Private;
            yield return IdeaPrivacy.Public;
        }

        public static readonly IdeaPrivacy Private = new IdeaPrivacy()
        {
            Id = 1,
            Name = "Private",
            Description = "Can be viewed only by owner"
        };

        public static readonly IdeaPrivacy Public = new IdeaPrivacy()
        {
            Id = 2,
            Name = "Public",
            Description = "Can be viewed by anybody"
        };
    }
}
