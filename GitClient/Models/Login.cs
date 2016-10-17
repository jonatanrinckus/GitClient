namespace GitClient.Models
{
	public class Login
	{
		public string Provider { get; set; }
		public string ProviderImg => $"../Resources/{Provider}.png";
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
