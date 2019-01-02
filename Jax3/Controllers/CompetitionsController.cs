using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jax3.Models;
using Jax3.Persistence;
using Jax3.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jax3.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly JaxDbContext context;
        private readonly IMapper mapper;

        public CompetitionsController(JaxDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompetition([FromBody] SaveCompetitionResource saveCompetitionResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var competition = mapper.Map<SaveCompetitionResource, Competition>(saveCompetitionResource);
            await context.Competitions.AddAsync(competition);
            await context.SaveChangesAsync();

            competition = await context.Competitions
                .Include(p => p.CreatedBy)
                .Include(p => p.Participants)
                    .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == competition.Id);

            return Ok(mapper.Map<Competition, ICompetitionResource>(competition));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompetition(int id, [FromBody] SaveCompetitionResource saveCompetitionResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var competition = await context.Competitions
                .Include(p => p.CreatedBy)
                .Include(p => p.Participants)
                    .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (competition == null)
                return NotFound();

            mapper.Map(saveCompetitionResource, competition);

            await context.SaveChangesAsync();

            return Ok(mapper.Map<Competition, ICompetitionResource>(competition));
        }

        [HttpGet]
        public async Task<IEnumerable<ICompetitionResourceShort>> GetCompetitions()
        {
            var competitions = await context.Competitions
                .Include(p => p.CreatedBy)
                .Include(p => p.Participants)
                .ToListAsync();

            return mapper.Map<List<Competition>, List<ICompetitionResourceShort>>(competitions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompetition(int id)
        {
            var competition = await context.Competitions
                .Include(p => p.CreatedBy)
                .Include(p => p.Participants)
                    .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (competition == null)
                return NotFound();

            return Ok(mapper.Map<Competition, ICompetitionResource>(competition));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var competition = await context.Competitions.FindAsync(id);

            if (competition == null)
                return NotFound();

            context.Remove(competition);
            await context.SaveChangesAsync();

            return Ok(id);
        }
    }
}