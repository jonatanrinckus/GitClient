using GitClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for CommentWindow.xaml
	/// </summary>
	public partial class CommentWindow : Window
	{
		private Issue Issue { get; set; }
		private ObservableCollection<Comment> Comments { get; set; }

		public CommentWindow(Issue issue, ObservableCollection<Comment> comments)
		{
			Issue = issue;
			Comments = comments;
			InitializeComponent();
		}

		private async void OnPostButtonClick(object sender, RoutedEventArgs e)
		{
			var comment = new Comment()
			{
				Body = $"{CommentTextBox.Text}\n\n" +
					   "<strong>Posted with GitClient by BeDatse.</strong>",
				CreatedAt = DateTimeOffset.UtcNow,
				User = App.AppManager.Composite.GetUserInfo().Result
			};


			if (!await App.AppManager.Composite.AddComment(Issue, comment))
			{
				MessageBox.Show("Error when trying to post the comment, please try again.");
				return;
			}
			else
			{
				MessageBox.Show("Comment posted successfully.");
			}

			Comments.Add(comment);
			Issue.Comments.Add(comment);

			Close();
		}
	}
}
