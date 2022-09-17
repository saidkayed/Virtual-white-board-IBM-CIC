using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whiteboard_API.Data;
using Whiteboard_API.Model;

namespace Whiteboard_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly Whiteboard_APIContext _context;

        public PostController(Whiteboard_APIContext context)
        {
            _context = context;
        }

        //get all posts by userid
        [HttpGet]
        [Route("/GetAllPostForUser/{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(int id)
        {
            return await _context.Post.Where(p => p.UserId == id).ToListAsync();
        }


        //get all posts with comments included
        [HttpGet]
        [Route("/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Post.Include(p => p.comments).ToListAsync();
        }

        //Create a post
        [HttpPost]
        [Route("/CreatePost")]
        public async Task<ActionResult<Post>> CreatePost([FromBody] PostDTO req)
        {

            Post post = new Post()
            {
                Title = req.Title,
                Content = req.Content,
                UserId = req.UserId
        };

            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }


        //delete post by id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }


        //give post a like
        [HttpPut]
        [Route("/LikePost{id}")]
        public async Task<ActionResult<Post>> LikePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Likes += 1;
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return post;
        }

    }
}
