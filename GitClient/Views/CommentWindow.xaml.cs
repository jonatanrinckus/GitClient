using GitClient.Models;
using System.Windows;

namespace GitClient.Views
{
	/// <summary>
	/// Interaction logic for CommentWindow.xaml
	/// </summary>
	public partial class CommentWindow : Window
	{
		private Issue Issue { get; set; }

		public CommentWindow(Issue issue)
		{
			Issue = issue;
			InitializeComponent();
		}
	}
}
