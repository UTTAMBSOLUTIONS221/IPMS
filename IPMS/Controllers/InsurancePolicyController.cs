
using IPMS.DbContexts;
using IPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InsurancePolicyController : ControllerBase
    {
        private readonly InsurancePolicyContext _context;

        public InsurancePolicyController(InsurancePolicyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsurancePolicy>>> GetPolicies()
        {
            var policies = await _context.Policies
                .Include(p => p.PolicyHolder)
                .ToListAsync();
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InsurancePolicy>> GetPolicy(int id)
        {
            var policy = await _context.Policies
                .Include(p => p.PolicyHolder)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (policy == null)
            {
                return NotFound(new { Message = "Policy not found" });
            }
            return Ok(policy);
        }

        [HttpPost]
        public async Task<ActionResult> AddPolicy([FromBody] InsurancePolicy policy)
        {
            var policyHolder = await _context.PolicyHolders.FindAsync(policy.PolicyHolderId);
            if (policyHolder == null)
            {
                return BadRequest(new { Message = "Policy holder does not exist" });
            }

            if (policy.StartDate >= policy.EndDate)
            {
                return BadRequest(new { Message = "Start date must be before end date" });
            }

            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolicy), new { id = policy.Id }, policy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] InsurancePolicy policy)
        {
            if (id != policy.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });
            }

            _context.Entry(policy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(id))
                {
                    return NotFound(new { Message = "Policy not found" });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound(new { Message = "Policy not found" });
            }

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PolicyExists(int id) => _context.Policies.Any(e => e.Id == id);
    }
}
