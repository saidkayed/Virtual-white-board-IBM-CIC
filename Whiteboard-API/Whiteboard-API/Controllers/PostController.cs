using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            return await _context.Post.Where(p => p.UserId == id).Include(p => p.comments).ToListAsync();
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
        [Route("/CreatePost"), Authorize]
        public async Task<ActionResult<Post>> CreatePost([FromBody] PostDTO req)
        {
            var username = User?.Identity?.Name;
            
            if (req.isAnonymous)
            {
                username = "anon";
            }
            
            Post post = new Post()
            {
                Title = req.Title,
                Content = req.Content,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Username = username
            };

            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        //delete post with the same user id
        [HttpDelete]
        [Route("/DeleteMyPost/{id}"), Authorize]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }


        //delete post by id
        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("/DeletePost/{id}")]
        public async Task<ActionResult<Post>> DeleteUserPost(int id)
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
        [HttpPut, Authorize]
        [Route("/LikePost/{id}")]
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
