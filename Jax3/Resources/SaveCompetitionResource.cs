using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Resources
{
    public class SaveCompetitionResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedById { get; set; }
        public ICollection<int> Participants { get; set; }

        public SaveCompetitionResource()
        {
            Participants = new Collection<int>();
        }
    }
}
