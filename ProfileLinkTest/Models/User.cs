using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProfileLinkTest.Models;

public class User
{
    public int UserId { get; set; }
	[Required]
	public string Username { get; set; } = null!;
	[Required]
    public string Password { get; set; } = null!;
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
	public string Bio { get; set; } = null!;
    public string Theme { get; set; } = null!;
	public ICollection<Social>? Socials { get; set; }
    public ICollection<Link>? Links { get; set; }
}
