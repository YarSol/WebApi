using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Models
{
    [Table("Competitions")]
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public ICollection<CompetitionUser> Participants { get; set; }

        public Competition()
        {
            Participants = new Collection<CompetitionUser>();
        }
    }
}
