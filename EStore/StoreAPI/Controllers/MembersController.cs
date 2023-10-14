using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class MembersController : ODataController
    {
        private readonly eStoreContext _context;

        public MembersController(eStoreContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            return await _context.Members.ToListAsync();
        }

        // GET: api/Members/5
        [EnableQuery]
        public async Task<ActionResult<Member>> GetMember([FromRoute] int key)
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            var member = await _context.Members.FindAsync(key);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public async Task<IActionResult> PutMember([FromRoute] int key, [FromBody] Member member)
        {
            if (key != member.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public async Task<ActionResult<Member>> PostMember([FromBody] Member member)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'eStoreContext.Members'  is null.");
            }
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/Members/5

        public async Task<IActionResult> DeleteMember([FromRoute] int key)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
