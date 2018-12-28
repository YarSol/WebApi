using Jax3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Resources
{
    public class CompetitionResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserResource CreatedBy { get; set; }
        public ICollection<UserResource> Participants { get; set; }
        public int ParticipantsAmount { get; set; }

        public CompetitionResource()
        {
            Participants = new Collection<UserResource>();
        }
    }
}
