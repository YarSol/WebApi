using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Models
{
    [Table("CompetitionUsers")]
    public class CompetitionUser
    {
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        public User User { get; set; }
        public Competition Competition { get; set; }        
    }
}
