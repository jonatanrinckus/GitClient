using System;
using System.Collections.Generic;

namespace GitClient.Models
{
	public class Issue
	{
		public string Body { get; set; }
		public User User { get; set; }
		public string Title { get; set; }
		public int Number { get; set; }
		public IList<Comment> Comments { get; set; }
		public ItemState State { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public int Id { get; set; }
		public Repository Repository { get; set; }
		public Uri CommentsUrl { get; set; }
		public Uri Url { get; set; }
	}
}
