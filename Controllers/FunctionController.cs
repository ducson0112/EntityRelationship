using Azure.Core;
using EntityRelationship.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityRelationship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        public readonly DataContext _dataContext;

        public FunctionController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Function>>> GetAllFunction ()
        {
            var functions = await _dataContext.Functions.ToListAsync();
            return Ok(functions);
        }

        [HttpPost]
        public async Task<ActionResult<Function>>  CreateFunction(CreateFunctionDTO request)
        {
            var newFunction = new Function()
            {
                Name = request.Name
            };

            await _dataContext.Functions.AddAsync(newFunction);
            await _dataContext.SaveChangesAsync();
            return Ok(newFunction);
        }

        [HttpPut("id")]
        public async Task<ActionResult<Function>> UpdateFunction(int id, CreateFunctionDTO request)
        {
            var function = await _dataContext.Functions
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (function == null)
            {
                return NotFound();
            }

            function.Name = request.Name;

            await _dataContext.SaveChangesAsync();
            return Ok(function);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Function>> DeleteFunction(int id)
        {
            var function = await _dataContext.Functions.FirstOrDefaultAsync(x => x.Id == id);

            if (function == null)
            {
                return NotFound();
            }
            else
            {
                _dataContext.Functions.Remove(function);
            }
            await _dataContext.SaveChangesAsync();
            return Ok(function);
        }

    }
}
