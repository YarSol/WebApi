using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jax3.Models;
using Jax3.Persistence;
using Jax3.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jax3.Controllers
{
    //[Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JaxDbContext context;
        private readonly IMapper mapper;

        public UsersController(JaxDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserResource>> GetUsers()
        {
            var users = await context.Users
                .Include(p => p.Competitions)
                    .ThenInclude(p => p.Competition)
                    .ThenInclude(p => p.CreatedBy)
                .ToListAsync();

            return mapper.Map<List<User>, List<UserResource>>(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await context.Users
                .Include(p => p.Competitions)
                    .ThenInclude(p => p.Competition)
                    .ThenInclude(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return NotFound();

            return Ok(mapper.Map<User, UserResource>(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] SaveUserResource userResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await context.Users
                .Include(p => p.Competitions)
                    .ThenInclude(p => p.Competition)
                    .ThenInclude(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (user == null)
                return NotFound();

            mapper.Map(userResource, user);

            await context.SaveChangesAsync();

            return Ok(mapper.Map<User, UserResource>(user));
        }
    }
}