using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Whiteboard_API.Data;
using Whiteboard_API.Model;
namespace Whiteboard_API.Controllers;




[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly Whiteboard_APIContext _context;
    IConfiguration _configuration;


    public AuthController(Whiteboard_APIContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    //get all users
    [HttpGet, Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.User.ToListAsync();
    }



    //create a new user for the platform
    [HttpPost]
    [Route("/Register")]
    public async Task<ActionResult<User>> Register([FromBody] UserDTO req)
    {
        if (!_context.User.Any(u => u.Username == req.Username))
        {

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        return NotFound();
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;

            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

             return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(User user)
    {

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.role.ToString()),  
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);


        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred);

        var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }



    [HttpPost]
    [Route("/Login")]
    public async Task<ActionResult<string>> Login([FromBody] UserDTO req)
    {

        // check if user exist
        if (_context.User.Any(u => u.Username == req.Username))
        {
            User user = _context.User.Single(u => u.Username == req.Username);

            if (VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt))
            {
                string token = CreateToken(user);
                
                return Ok(token);

            }
        }
        return BadRequest("Account not found.");
    }



    /*
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
    */
}
