using Jax3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Resources
{
    public class UserResource
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<CompetitionResource> Competitions { get; set; }
        public int CompetitionsAmount { get; set; }

        public UserResource()
        {
            Competitions = new Collection<CompetitionResource>();
        }        
    }
}
