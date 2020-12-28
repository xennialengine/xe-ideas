using System.Collections.Generic;

namespace xe_ideas.Models
{
    public class ApplicationContext
    {
        // TODO add more data for context

        public ApplicationUser CurrentUser { get; set; }

        public IDictionary<string, object> Data { get; set; }
    }
}
