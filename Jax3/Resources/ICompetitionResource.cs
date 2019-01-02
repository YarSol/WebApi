using System.Collections.Generic;

namespace Jax3.Resources
{
    public interface ICompetitionResource
    {
        int Id { get; set; }
        string Name { get; set; }
        IUserResourceShort CreatedBy { get; set; }
        ICollection<IUserResourceShort> Participants { get; set; }
        int ParticipantsAmount { get; set; }
    }
}
