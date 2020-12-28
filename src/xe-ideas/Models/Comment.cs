using System;
using System.Text.Json.Serialization;

namespace xe_ideas.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        [JsonIgnore]
        public ApplicationUser Creator { get; set; }

        public int IdeaId { get; set; }

        [JsonIgnore]
        public Idea Idea { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
