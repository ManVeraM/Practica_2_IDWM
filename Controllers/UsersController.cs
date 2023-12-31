using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica2_IDWM.Models;

namespace Practica2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User newUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = newUser.Name;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            user.City = newUser.City;
            user.Country = newUser.Country;
            user.Summary = newUser.Summary;

            await _context.SaveChangesAsync();
            return NoContent();


           

        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Obtener el perfil desde la base de datos
            var userProfile = _context.Users
                .Include(u => u.Frameworks)
                .Include(u => u.Hobbies)
                .FirstOrDefault();

            if (userProfile == null)
            {
                return NotFound("Perfil no encontrado");
            }



            // Construcción del objeto JSON de respuesta
            var jsonResponse = new
            {
                id = userProfile.Id,
                Name = userProfile.Name,
                Lastname = userProfile.LastName,
                Email = userProfile.Email,
                City = userProfile.City,
                Country = userProfile.Country,
                Summary = userProfile.Summary,
                Frameworks = userProfile.Frameworks,
                Hobbies = userProfile.Hobbies
            };

            // Devuelve el JSON como resultado
            
            return Ok(jsonResponse);
        }
                
        [HttpPut("hobbies/{hobbieId}")]
        public async Task<IActionResult> UpdateUserHobbie(long userId, long hobbieId, Hobbie updatedHobbie)
        {
            var hobbie = await _context.Hobbies.FirstOrDefaultAsync(h => h.Id == hobbieId);
            if (hobbie == null)
            {
                return NotFound("Hobbie no encontrado");
            }

            hobbie.Name = updatedHobbie.Name;
            hobbie.Description = updatedHobbie.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("frameworks/{frameworkId}")]
        public async Task<IActionResult> UpdateUserFramework(int frameworkId, Framework updatedFramework)
        {

            var framework = await _context.Frameworks.FirstOrDefaultAsync(f => f.Id == frameworkId);
            if (framework == null)
            {
                return NotFound("no existe el framework");
            }
            framework.Name = updatedFramework.Name;
            framework.Level = updatedFramework.Level;
            framework.Year = updatedFramework.Year;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
