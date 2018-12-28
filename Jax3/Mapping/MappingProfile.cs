using AutoMapper;
using Jax3.Models;
using Jax3.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Competition, SaveCompetitionResource>()
                .ForMember(p => p.Participants, opt => opt.MapFrom(v => v.Participants.Select(p => p.UserId)));
            CreateMap<User, SaveUserResource>()
                .ForMember(p => p.Competitions, opt => opt.MapFrom(v => v.Competitions.Select(p => p.CompetitionId)));

            CreateMap<Competition, CompetitionResource>()
                 .ForMember(p => p.ParticipantsAmount, opt => opt.MapFrom(v => v.Participants.Count))
                 .ForMember(p => p.Participants, opt => opt.MapFrom(v => v.Participants.Select(p => new UserResource
                 {
                     Id = p.UserId,
                     FirstName = p.User.FirstName,
                     LastName = p.User.LastName,
                     Email = p.User.Email
                 })));


            CreateMap<User, UserResource>()
                 .ForMember(p => p.CompetitionsAmount, opt => opt.MapFrom(v => v.Competitions.Count))
                 .ForMember(p => p.Competitions, opt => opt.MapFrom(v => v.Competitions.Select(p => new CompetitionResource
                 {
                     Id = p.CompetitionId,
                     Name = p.Competition.Name,
                     CreatedBy = new UserResource
                     {
                         Id = p.Competition.CreatedBy.Id,
                         FirstName = p.Competition.CreatedBy.FirstName,
                         LastName = p.Competition.CreatedBy.LastName,
                         Email = p.Competition.CreatedBy.Email
                     }
                 })));

            // API Resource to Domain
            CreateMap<SaveCompetitionResource, Competition>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Participants, opt => opt.Ignore())
                .AfterMap((r, o) =>
                {
                    // Remove unselected participants                    
                    var removedParticipants = o.Participants.Where(p => !r.Participants.Contains(p.UserId)).ToList();
                    foreach (var u in removedParticipants)
                        o.Participants.Remove(u);

                    // Add new participants
                    var addedParticipants = r.Participants.Where(id => !o.Participants.Any(p => p.UserId == id)).Select(id => new CompetitionUser { UserId = id });
                    foreach (var cu in addedParticipants)
                        o.Participants.Add(cu);
                });

            CreateMap<SaveUserResource, User>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Competitions, opt => opt.Ignore())
                .AfterMap((r, o) =>
                {
                    // Remove unselected competitions                    
                    var removedCompetitions = o.Competitions.Where(p => !r.Competitions.Contains(p.CompetitionId)).ToList();
                    foreach (var u in removedCompetitions)
                        o.Competitions.Remove(u);

                    // Add new competitions
                    var addedCompetitions = r.Competitions.Where(id => !o.Competitions.Any(p => p.CompetitionId == id)).Select(id => new CompetitionUser { CompetitionId = id });
                    foreach (var cu in addedCompetitions)
                        o.Competitions.Add(cu);
                });


        }
    }
}
