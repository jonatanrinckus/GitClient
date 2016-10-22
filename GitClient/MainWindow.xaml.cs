using GitClient.ViewModels;
using GitClient.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace GitClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel Context { get; }

		public MainWindow()
		{
			InitializeComponent();
			Context = ((MainViewModel)DataContext);
			Context.Status = "Starting...";

			//  DispatcherTimer setup
			var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			dispatcherTimer.Start();

			Load();
		}

		private async void Load()
		{
			var inUseAdapter = App.AppManager.Composite.InUse;
			Context.Status = $"Loading repositorios...";
			await inUseAdapter.LoadRepositories().ContinueWith(t =>
			{
				if (t.IsCompleted)
				{
					Context.Status = $"Loading issues...";
					inUseAdapter.LoadIssues().ContinueWith(t2 =>
					{
						if (t2.IsCompleted)
							Context.Status = $"Welcome {inUseAdapter.GetUserInfo().Name}, " +
											 $"you are logged in {inUseAdapter.GetLoginInfo().Provider} " +
											 $"as {inUseAdapter.GetLoginInfo().Username}";
						else
							Context.Status = $"Error to load issues";
					});
				}
				else
					Context.Status = $"Error to load repositories";

			});




		}

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			// Updating the Label which displays the current second
			Context.Clock = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second:D2}";

			// Forcing the CommandManager to raise the RequerySuggested event
			CommandManager.InvalidateRequerySuggested();
		}

		private void OnIssueMenuClick(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new IssuePage());
		}
	}
}
