using GitClient.Models;
using System.Windows;
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

		private void OnCommentButtonClick(object sender, RoutedEventArgs e)
		{

			var commentWindow = new CommentWindow(Issue);

			commentWindow.ShowDialog();
		}
	}
}
