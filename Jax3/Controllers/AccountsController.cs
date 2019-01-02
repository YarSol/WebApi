using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jax3.Models;
using Jax3.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using AutoMapper;
using Jax3.Resources;

namespace Jax3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly JaxDbContext context;
        private readonly IMapper mapper;

        public AccountsController(JaxDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<object> Signup([FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isExistingUser = context.Users.Where(x => x.Email == model.Email);

            if (isExistingUser.Count() > 0)
            {
                ModelState.AddModelError("Email", "The user with email '" + model.Email + "' already exists.");
                return BadRequest(ModelState);
            }

            await context.Users.AddAsync(model);
            await context.SaveChangesAsync();

            var user = await context.Users
                .Include(p => p.Competitions)
                    .ThenInclude(p => p.Competition)
                    .ThenInclude(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            return GetToken(mapper.Map<User, IUserResourceShort>(user));
        }

        [HttpPost]
        public async Task<object> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await context.Users
                .Include(p => p.Competitions)
                    .ThenInclude(p => p.Competition)
                    .ThenInclude(p => p.CreatedBy)
                .SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "The user with email '" + model.Email + "' is not registered yet. Please, register first.");
                return BadRequest(ModelState);
            }

            return GetToken(mapper.Map<User, IUserResourceShort>(user));
        }

        private object GetToken(IUserResourceShort model)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.FirstName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, model.LastName)
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email,
                currentUserId = model.Id
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}