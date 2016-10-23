using GitClient.Models;
using System.Windows.Controls;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for IssueDetailPage.xaml
	/// </summary>
	public partial class IssueDetailPage : Page
	{
		public Issue Issue { get; }

		public IssueDetailPage(Issue issue)
		{
			Issue = issue;
			InitializeComponent();
		}
	}
}
