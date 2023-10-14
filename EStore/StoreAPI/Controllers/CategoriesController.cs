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
    public class CategoriesController : ODataController
    {
        private readonly eStoreContext _context;

        public CategoriesController(eStoreContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            return await _context.Categories.ToListAsync();
        }
    }
}
