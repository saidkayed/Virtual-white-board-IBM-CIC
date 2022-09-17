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
    public class CommentsController : ControllerBase
    {
        private readonly Whiteboard_APIContext _context;

        public CommentsController(Whiteboard_APIContext context)
        {
            _context = context;
        }

        //add new comment to post use dto
        [HttpPost, Authorize]
        public async Task<ActionResult<Comment>> PostComment(CommentDTO commentDTO)
        {
            var comment = new Comment
            {
                Username = User?.Identity?.Name,
                PostId = commentDTO.PostId,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Content = commentDTO.Content,
                Date = DateTime.Now
              
            };
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

      
        //delete comment
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
    }

}
