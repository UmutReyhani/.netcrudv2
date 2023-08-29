using CrudMongo.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CrudMongo.Models;
using MongoDB.Bson;

namespace CrudMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConnectionService _connectionService;

        private IMongoCollection<Category> _categoryCollection;

        public TestController(IConnectionService connectionService)
        {
            _connectionService = connectionService;
            _categoryCollection = _connectionService.db().GetCollection<Category>("CategoryCollection");
        }

        #region GetCategory
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            var categories = await _categoryCollection.AsQueryable().ToListAsync();
            return Ok(categories);
        }
        #endregion

        #region GetCategoryById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        #endregion

        #region PostCategory
        [HttpPost]
        public async Task<IActionResult> PostCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Id))
            {
                category.Id = ObjectId.GenerateNewId().ToString();
            }

            await _categoryCollection.InsertOneAsync(category);
            return Ok("created successfully");
        }
        #endregion

        #region UpdateCategory
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category updatedCategory)
        {
            var category = await _categoryCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            await _categoryCollection.ReplaceOneAsync(a => a.Id == id, updatedCategory);
            return Ok("updated successfully");
        }
        #endregion

        #region DeleteCategory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _categoryCollection.Find(a => a.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            await _categoryCollection.DeleteOneAsync(a => a.Id == id);
            return Ok("deleted successfully");
        }
        #endregion
    }
}
