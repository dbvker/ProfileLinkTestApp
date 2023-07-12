using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileLinkTest.Models;

namespace ProfileLinkTest.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous] // Remove this later
public class SocialsController : ControllerBase
{
	private readonly UserContext _context;
	public SocialsController(UserContext context)
	{
		_context = context;
	}

	[HttpGet] // Delete this Route later
	public async Task<ActionResult<ICollection<Social>>> Get()
	{
		var socials = await _context.Socials
			.ToListAsync();

		return Ok(socials);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetSocialById(int id)
	{
		var social = await _context.Socials
			.Where(s => s.SocialId == id)
			.FirstOrDefaultAsync();

		if (social == null)
		{
			return NotFound($"Social with id '{id}' doesn't exist.");
		}

		return Ok(social);
	}

	public record SocialData(string Platform, string Username);
	[HttpPost]
	public async Task<IActionResult> CreateSocial(int userId, [FromBody] SocialData data)
	{
		if (data.Platform == null || data.Username == null)
		{
			return BadRequest("Platform and username required.");
		}

		var user = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();
		if (user == null)
		{
			return NotFound();
		}
		else
		{
			var newSocial = new Social
			{
				Platform = data.Platform,
				Username = data.Username,
				User = user
			};

			_context.Socials.Add(newSocial);
			await _context.SaveChangesAsync();

			return Ok();
		}

	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateSocial(int id, [FromBody] SocialData data)
	{
		if (data.Platform == null || data.Username == null)
		{
			return BadRequest();
		}

		var socialItem = _context.Socials.Where(s => s.SocialId == id).FirstOrDefault();
		if (socialItem == null)
		{
			return NotFound();
		}

		socialItem.Platform = data.Platform;
		socialItem.Username = data.Username;

		try
		{
			await _context.SaveChangesAsync();
			return NoContent();
		}
		catch (Exception)
		{
			return NotFound();
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteSocial(int id)
	{
		var social = await _context.Socials
			.Where(s => s.SocialId == id)
			.FirstOrDefaultAsync();

		if (social == null)
		{
			return NotFound();
		}

		_context.Socials.Remove(social);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}
