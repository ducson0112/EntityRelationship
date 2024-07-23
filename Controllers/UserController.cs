using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityRelationship.DTOs;

namespace EntityRelationship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var users = await _dataContext.Users
                .Include(x => x.Roles)
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("Id")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _dataContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(CreateUserDTO request)
        {
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email
            };

            await _dataContext.Users.AddAsync(newUser);
            await _dataContext.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPost("role")]
        public async Task<ActionResult<User>> AddUserRole(int userId, int roleId)
        {
            var user = await _dataContext.Users
                .Where(u => u.Id == userId)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            var role = await _dataContext.Roles.FindAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            user.Roles.Add(role);
            await _dataContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("id")]
        public async Task<ActionResult<User>> UpdateUser(CreateUserDTO request, int id)
        {
            var user = await _dataContext.Users
                .Where(u => u.Id == id)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            user.Id = id;
            user.Name = request.Name;
            user.Email = request.Email;

            await _dataContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _dataContext.Users
               .Where(u => u.Id == id)
               .Include(x => x.Roles)
               .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            } else
            {
                _dataContext.Users.Remove(user);
            }

            await _dataContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("role")]
        public async Task<ActionResult<User>> DeleteUserRole(int userId, int roleId)
        {
            var user = await _dataContext.Users
                .Where(u => u.Id == userId)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            var role = await _dataContext.Roles.FindAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            user.Roles.Remove(role);
            await _dataContext.SaveChangesAsync();
            return Ok(user);
        }
    }
}
