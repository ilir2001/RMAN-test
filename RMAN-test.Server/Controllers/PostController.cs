using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RMAN_test.Server.Data;
using RMAN_test.Server.Models;

namespace RMAN_test.Server.Controllers
{
    public class PostController: ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public PostController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("api/posts")]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _dbContext.Posts.Find(_ => true).ToListAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while retrieving posts.");
            }
        }

        [Route("api/post")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            await _dbContext.Posts.InsertOneAsync(post);
            return Ok(post);
        }
    }
}
