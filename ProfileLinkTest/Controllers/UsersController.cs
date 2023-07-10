using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileLinkTest.Models;
using ProfileLinkTest.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ProfileLinkTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly UserContext _context;
	private readonly IConfiguration _config;

	public UsersController(UserContext context, IConfiguration config)
	{
		_context = context;
		_config = config;
	}

	// Get all users - Maybe delete later
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<ICollection<UserDTO>>> Get()
	{
		var users = await _context.Users
			.Include(u => u.Socials)
			.Include(u => u.Links)
			.Select(x => userDTO(x))
			.ToListAsync();

		return Ok(users);
	}

	// Get exact user by id
	[HttpGet("{id}")]
	public async Task<ActionResult<UserDTO>> Get(int id)
	{
		var user = await _context.Users
			.Where(u => u.UserId == id)
			.Include(u => u.Socials)
			.Include(u => u.Links)
			.Select(x => userDTO(x))
			.FirstOrDefaultAsync();

		if (user == null)
		{
			return NotFound($"User with id '{id}' doesn't exist.");
		}

		return Ok(user);
	}

	[HttpGet("profile/{username}")]
	public async Task<ActionResult<UserDTO>> GetByUsername(string username)
	{
		var user = await _context.Users
			.Where(u => u.Username.ToLower() == username.ToLower())
			.Include(u => u.Socials)
			.Include(u => u.Links)
			.Select(x => userDTO(x))
			.FirstOrDefaultAsync();

		if (user == null)
		{
			return NotFound($"User with username '{username}' doesn't exist.");
		}

		return Ok(user);
	}

	// Create user
	public record RegistrationData(string? FirstName, string? LastName, string? Username, string? Password);

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult<UserDTO>> Post([FromBody] RegistrationData _userData)
	{
		if (_userData != null && 
			_userData.Username != null && 
			_userData.Password != null && 
			_userData.FirstName != null && 
			_userData.LastName != null)
		{
			var user = await GetUser(_userData.Username);

			if (user == null)
			{
				var newUser = new User
				{
					Username = _userData.Username!,
					Password = _userData.Password!,
					FirstName = _userData.FirstName!,
					LastName = _userData.LastName!,
					Bio = ""!,
					Theme = "840AD7"
				};

				_context.Users.Add(newUser);
				await _context.SaveChangesAsync();

				return CreatedAtAction(
					nameof(GetByUsername),
					new { username = newUser.Username },
					userDTO(newUser));
			}
			else
			{
				return BadRequest();
			}
		}
		else
		{
			return BadRequest();
		}
	}

	// Delete User
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteUser(int id)
	{
		var user = await _context.Users
			.Where(u => u.UserId == id)
			.FirstOrDefaultAsync();

		if (user == null)
		{
			return NotFound();
		}

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private static UserDTO userDTO(User user) => new UserDTO
	{
		UserId = user.UserId,
		Username = user.Username,
		FirstName = user.FirstName,
		LastName = user.LastName,
		Bio = user.Bio,
		Theme = user.Theme,
		Socials = user.Socials,
		Links = user.Links
	};

	private async Task<User> GetUser(string username)
	{
		var user = await _context.Users
			.Where(u => u.Username.ToLower() == username.ToLower())
			.FirstOrDefaultAsync();

		return user!;
	}
}
