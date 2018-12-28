using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Resources
{
    public class SaveUserResource
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<int> Competitions { get; set; }

        public SaveUserResource()
        {
            Competitions = new Collection<int>();
        }
    }
}
