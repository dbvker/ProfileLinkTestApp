using Microsoft.AspNetCore.Mvc;
using ProfileLinkTest.Models;

namespace ProfileLinkTest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LinksController : ControllerBase
	{
		private readonly UserContext _context;

		public LinksController(UserContext context)
        {
			_context = context;
		}

		[HttpGet("{id}")]
		public void GetLinkById(int id)
		{
			throw new NotImplementedException();
		}
    }
}
