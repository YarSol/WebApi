namespace Jax3.Resources
{
    public interface ICompetitionResourceShort
    {
        int Id { get; set; }
        string Name { get; set; }
        IUserResourceShort CreatedBy { get; set; }
        int ParticipantsAmount { get; set; }
    }
}
