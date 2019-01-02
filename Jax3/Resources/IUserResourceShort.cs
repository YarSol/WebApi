using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Resources
{
    public interface IUserResourceShort
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        int CompetitionsAmount { get; set; }
    }
}
