using System.Collections.Generic;

namespace GitClient.Models
{
	public class Repository
	{
		public Repository()
		{
			Issues = new List<Issue>();
		}
		public long Id { get; set; }
		public string Name { get; set; }
		public string FullName { get; set; }
		public bool HasIssues { get; set; }
		public List<Issue> Issues { get; set; }
		public string Language { get; set; }
	}
}
