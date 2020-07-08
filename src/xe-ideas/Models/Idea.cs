using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using xe_ideas.Models.LookUp;

namespace xe_ideas.Models
{
    public class Idea
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        [JsonIgnore]
        public ApplicationUser Creator { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public int PrivacyId { get; set; } = 1;
        public IdeaPrivacy Privacy { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; }
    }
}
