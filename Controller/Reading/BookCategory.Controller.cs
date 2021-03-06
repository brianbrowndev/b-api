using System.Linq;
using Microsoft.AspNetCore.Mvc;
using B.API.Database;
using Microsoft.AspNetCore.Authorization;
using B.API.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace B.API.Controller
{

    [Route("v1/reading/categories")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BookCategoryController: AppControllerBase
    {
        private readonly LookupRepository _lookupRepository;
        private readonly AppDbContext _context;

        private readonly ILogger _logger;
        public BookCategoryController(AppDbContext context, ILogger<BookCategoryController> logger,  LookupRepository lookupRepository): base(context, logger)
        {
            _lookupRepository = lookupRepository;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public ActionResult<IEnumerable<BookCategory>> GetCategories()
        {
            return Ok(_context.BookCategory.AsNoTracking().OrderBy(c => c.Name));
        }
        [Authorize]
        [HttpGet("page")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public ActionResult<PaginatedResult<BookCategory>> GetCategoriesPage(
            [FromQuery]string sortName,
            [FromQuery]int pageNumber = 1,
            [FromQuery]int pageSize = 25
        ) 
        {
            var items = _lookupRepository.OrderBy<BookCategory>(_context.BookCategory.AsNoTracking(), sortName);
            return Ok(_lookupRepository.Paginate(items, pageNumber, pageSize));
        }
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public ActionResult<BookCategory> GetCategory(int id)
        {
            return Ok(_context.BookCategory.AsNoTracking().First(o => o.Id == id));
        }
        [Authorize]
        [HttpPost]
        public ActionResult<BookCategory> CreateCategory([FromBody] BookCategory item)
        {
            return Create<BookCategory>(item, nameof(CreateCategory));
        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] BookCategory item)
        {
            return Update<BookCategory>(id, item);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory (int id)
        {
            return Delete<BookCategory>(id);
        }


    }
}

