using GitClient.Models;
using System.Collections.ObjectModel;
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
		public ObservableCollection<Comment> Comments { get; }
		public IssueDetailPage(Issue issue)
		{
			Issue = issue;
			Comments = new ObservableCollection<Comment>(issue.Comments);
			InitializeComponent();
		}

		private void OnCommentButtonClick(object sender, RoutedEventArgs e)
		{

			var commentWindow = new CommentWindow(Issue, Comments);

			commentWindow.ShowDialog();

		}

		private void OnCloseIssueButtonClick(object sender, RoutedEventArgs e)
		{
			App.AppManager.Composite.CloseIsse(Issue);
		}
	}
}
