using System.Text.Json.Serialization;

namespace ProfileLinkTest.Models;

public class Link
{
    public int LinkId { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
	public bool Active { get; set; }
	public int UserId { get; set; }
	[JsonIgnore]
	public User? User { get; set; }
}
