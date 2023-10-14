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
    [Route("odata")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private readonly eStoreContext _context;

        public ProductsController(eStoreContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [EnableQuery]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int key)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(key);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public async Task<IActionResult> PutProduct([FromRoute] int key, [FromBody] Product product)
        {
            if (key != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'eStoreContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        public async Task<IActionResult> DeleteProduct([FromRoute] int key)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
