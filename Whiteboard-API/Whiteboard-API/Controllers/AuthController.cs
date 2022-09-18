﻿using Microsoft.AspNetCore.Authorization;
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

    //change user username
    [HttpPut, Authorize]
    [Route("/ChangeUsername")]
    public async Task<ActionResult<User>> ChangeUsername(ChangeUsernameDTO req)
    {
        var user = await _context.User.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        if (user == null)
        {
            return NotFound();
        }

        if (user.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            return Unauthorized();
        }

        user.Username = req.Username;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    //get all users
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("/GetAllUsers")]
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

    //register admin user
    [HttpPost]
    [Route("/RegisterAdmin")]
    public async Task<ActionResult<User>> RegisterAdmin([FromBody] UserDTO req)
    {
        if (!_context.User.Any(u => u.Username == req.Username))
        {

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = req.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                role = Role.Admin
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        return NotFound();
    }

    //reset password
    [HttpPost]
    [Route("/ResetPassword")]
    public async Task<ActionResult<User>> ResetPassword([FromBody] UserDTO req)
    {
        if (_context.User.Any(u => u.Username == req.Username))
        {
            var user = _context.User.FirstOrDefault(u => u.Username == req.Username);

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.User.Update(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        return NotFound();
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



}
