using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Whiteboard_API.Data;
using Whiteboard_API.Interfaces;
using Whiteboard_API.Model;
namespace Whiteboard_API.Controllers;




[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IUserRepository _userRepository;
    IConfiguration _configuration;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public AuthController(IUserRepository mockUserRepository)
    {
        _userRepository = mockUserRepository;
    }


    //change user username
    [HttpPut, Authorize]
    [Route("/ChangeUsername")]
    public async Task<ActionResult<User>> ChangeUsername(ChangeUsernameDTO req)
    {
        var user = await _userRepository.GetUserById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        if (user == null)
        {
            return NotFound();
        }

        if (user.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            return Unauthorized();
        }

        user.Username = req.Username;

        await _userRepository.UpdateUser(user);

        return Ok(user);
    }

    //get all users
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("/GetAllUsers")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();

        return Ok(users);
    }




    //create a new user for the platform
    [HttpPost]
    [Route("/Register")]
    public async Task<ActionResult<User>> Register([FromBody] UserDTO req)
    {
        if (!_userRepository.DoUsernameExist(req.Username).Result)
        {

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.CreateUser(user);


            return Ok(user);
        }
        return NotFound();
    }

    //register admin user
    [HttpPost]
    [Route("/RegisterAdmin")]
    public async Task<ActionResult<User>> RegisterAdmin([FromBody] UserDTO req)
    {
        if (!_userRepository.DoUsernameExist(req.Username).Result)
        {

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                role = Role.Admin
            };

            await _userRepository.CreateUser(user);

            return Ok(user);
        }
        return NotFound();
    }

    //reset password
    [HttpPost]
    [Route("/ResetPassword")]
    public async Task<ActionResult<User>> ResetPassword([FromBody] UserDTO req)
    {
        if (_userRepository.DoUsernameExist(req.Username).Result)
        {
            var user = await _userRepository.GetUserByUsername(req.Username);

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateUser(user);

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

    /*
    //admin add new user
    [HttpPost, Authorize(Roles = "Admin")]
    [Route("/AddUser")]
    public async Task<ActionResult<User>> AddUser([FromBody] UserDTO req)
    {
        if (!_context.User.Any(u => u.Username == req.Username))
        {

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                role = Role.User
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        return NotFound();
    }
    */

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
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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
        if (_userRepository.DoUsernameExist(req.Username).Result)
        {
            User user = await _userRepository.GetUserByUsername(req.Username);

            if (VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt))
            {
                string token = CreateToken(user);

                return Ok(token);

            }
        }
        return BadRequest("Account not found.");
    }
}
