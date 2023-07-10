using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileLinkTest.Models;

namespace ProfileLinkTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SocialsController : ControllerBase
{
	private readonly UserContext _context;
	public SocialsController(UserContext context)
	{
		_context = context;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Social>> GetSocialById(int id)
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

	public record SocialData(string? platform, string? username);
	[HttpPost]
	public async Task<ActionResult<Social>> CreateSocial(int? id, [FromBody] SocialData data)
	{
		if (data.platform == null || data.username == null || id == null)
		{
			return BadRequest("Platform and username and user id required.");
		}

		var user = _context.Users.Where(u => u.UserId == id).FirstOrDefault();
		var newSocial = new Social
		{
			Username = data.username,
			Platform = data.platform,
			User = user!
		};

		_context.Socials.Add(newSocial);
		await _context.SaveChangesAsync();

		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteSocial(int id)
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
