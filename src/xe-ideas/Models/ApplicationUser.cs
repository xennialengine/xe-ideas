using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xe_ideas.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public ICollection<Idea> Ideas { get; set; }
    }
}
