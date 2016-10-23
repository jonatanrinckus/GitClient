using System;

namespace GitClient.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string Body { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public User User { get; set; }
	}
}
