using Azure.Core;
using EntityRelationship.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityRelationship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public readonly DataContext _dataContext;

        public RoleController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetAllRole()
        {
            var roles = await _dataContext.Roles
                .Include(r => r.Functions)
                .ToListAsync();
            return Ok(roles);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Role>> GetRoleById (int id) 
        {
            var role = await _dataContext.Roles
                .Include(r => r.Functions)
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole (CreateRoleDTO request)
        {
            
            var newRole = new Role()
            {
                Name = request.Name
            };
            await _dataContext.AddAsync(newRole);
            await _dataContext.SaveChangesAsync();

            return Ok(newRole);
        }

        [HttpPost("Function")]
        public async Task<ActionResult<Role>> CreateRoleFunction(int RoleId, int FunctionId)
        {

            var newRole = await _dataContext.Roles
                .Where(x => x.Id == RoleId)
                .Include(r => r.Functions)
                .FirstOrDefaultAsync();
            if (newRole == null)
            {
                return BadRequest();
            }

            var function = await _dataContext.Functions
                .Where(f => f.Id == FunctionId)
                .FirstOrDefaultAsync();

            if (function == null)
            {
                return BadRequest();
            }

            newRole.Functions.Add(function);
            await _dataContext.SaveChangesAsync();

            return Ok(newRole);
        }

        [HttpPut("id")] 
        public async Task<ActionResult<Role>> updateRole(CreateRoleDTO request, int id)
        {
            var newRole = await _dataContext.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (newRole == null)
            {
                return BadRequest();
            }
            newRole.Name = request.Name;
            
            await _dataContext.SaveChangesAsync();

            return Ok(newRole);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Role>> DeleteRole(int id)
        {
            var newRole = await _dataContext.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (newRole == null)
            {
                return BadRequest();
            }
            _dataContext.Roles.Remove(newRole);

            await _dataContext.SaveChangesAsync();

            return Ok(newRole);
        }

        [HttpDelete("Function")]
        public async Task<ActionResult<Role>> DeleteRoleFunction(int RoleId, int FunctionId)
        {

            var role = await _dataContext.Roles
                .Where(x => x.Id == RoleId)
                .Include(r => r.Functions)
                .FirstOrDefaultAsync();
            if (role == null)
            {
                return BadRequest();
            }

            var function = await _dataContext.Functions
                .Where(f => f.Id == FunctionId)
                .FirstOrDefaultAsync();

            if (function == null)
            {
                return BadRequest();
            }

            role.Functions.Remove(function);
            await _dataContext.SaveChangesAsync();

            return Ok(role);
        }
    }
}
