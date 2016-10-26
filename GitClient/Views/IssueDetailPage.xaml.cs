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
		public IssuePage IssuePage { get; set; }
		public ObservableCollection<Comment> Comments { get; }
		public IssueDetailPage(Issue issue, IssuePage issuePage)
		{
			Issue = issue;
			IssuePage = issuePage;
			Comments = new ObservableCollection<Comment>(issue.Comments);
			InitializeComponent();
			OpenCloseIssueButton.Content = Issue.State == ItemState.Open ? "Close Issue" : "Reopen Issue";
		}

		private void OnCommentButtonClick(object sender, RoutedEventArgs e)
		{

			var commentWindow = new CommentWindow(Issue, Comments);

			commentWindow.ShowDialog();

		}

		private async void OnOpenCloseIssueButtonClick(object sender, RoutedEventArgs e)
		{

			OpenCloseIssueButton.Content = Issue.State == ItemState.Open ? "Reopen Issue" : "Close Issue";

			var state = Issue.State == ItemState.Open ? "closed" : "open";

			if (await App.AppManager.Composite.ChangeIssueState(Issue, state))
			{
				Issue.State = Issue.State == ItemState.Open ? ItemState.Closed : ItemState.Open;
				IssuePage.Load();
			}
		}
	}
}
