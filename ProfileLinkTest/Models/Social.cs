using System.Text.Json.Serialization;

namespace ProfileLinkTest.Models;

public class Social
{
    public int SocialId { get; set; }
    public string? Platform { get; set; }
    public string? Username { get; set; }
	[JsonIgnore]
	public User? User { get; set; }
}
