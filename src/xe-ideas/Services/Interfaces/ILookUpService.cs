using System.Collections.Generic;
using xe_ideas.Models;

namespace xe_ideas.Services.Interfaces
{
    public interface ILookUpService
    {
        /// <summary>
        /// Returns a dictionary of all {lookup-name, list-of-lookup-values}
        /// </summary>
        /// <returns></returns>
        IDictionary<string, IEnumerable<BaseLookUpModel>> GetAll(ApplicationContext context);
    }
}
