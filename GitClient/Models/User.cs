namespace GitClient.Models
{
	public class User
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public Provider Provider { get; set; }
		public string Username { get; set; }
		public string AvatarUrl { get; set; }
	}
}
