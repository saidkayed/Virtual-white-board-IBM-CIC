using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whiteboard_API.Data;
using Whiteboard_API.Model;
namespace Whiteboard_API.Controllers;



[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly Whiteboard_APIContext _context;


    public UserController(Whiteboard_APIContext context)
    {
        _context = context;
    }

    //get all users 
    [HttpGet]
    [Route("/Users")]
    public async Task<ActionResult<IEnumerable<User>>> GetCatalogs()
    {
        return await _context.User.ToListAsync();
    }
    

    //create a new user for the platform
    [HttpPost]
    [Route("/CreateUser")]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCatalogs", new { id = user.Id }, user);
    }


    [HttpPost]
    [Route("/Login")]
    public async Task<ActionResult<User>> Login([FromBody] User _user)
    {
        var user = _user;

        // check if user exist
        if (_context.User.Any(u => u.Username == user.Username))
        {

            user = _context.User.First(u => u.Username == user.Username);

            if (_user.Password != user.Password)
            {
                return NotFound();
            }

            return user;
        }
        return NotFound();
        
    }

    [HttpPost]
    [Route("/ResetPassword")]
    public async Task<ActionResult<User>> Reset([FromBody] User _user)
    {
        var user = _user;

        // check if user exist
        if (_context.User.Any(u => u.Username == user.Username))
        {

            user = _context.User.First(u => u.Username == user.Username);

            if (_user.Password != user.Password)
            {
                return NotFound();
            }

            return user;
        }
        return NotFound();

    }

}
