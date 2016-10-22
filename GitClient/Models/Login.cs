namespace GitClient.Models
{
	public class Login
	{
		public Provider Provider { get; set; }
		public string ProviderImg => $"../Resources/{Provider}.png";
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
