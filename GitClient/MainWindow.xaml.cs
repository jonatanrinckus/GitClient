using GitClient.ViewModels;
using GitClient.Views;
using System;
using System.Windows;
using System.Windows.Input;
using GitClient.ServiceReference1;

namespace GitClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static MainViewModel Context { get; set; }

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

			SetStatus();


		}

		public static void SetStatus()
		{
			var userInfo = App.AppManager.Composite.InUse.GetUserInfo().Result;
			Context.Status = $"Welcome {userInfo.Name}, " +
											 $"you are logged in {userInfo.Provider} " +
											 $"as {userInfo.Username}";
		}

		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			// Updating the Label which displays the current second

			IService1 service = new Service1Client();

			var date = service.GetDateTimeUtcNow().ToLocalTime();
			
			Context.Clock = $"{date.Hour:D2}:{date.Minute:D2}:{date.Second:D2}";

			// Forcing the CommandManager to raise the RequerySuggested event
			CommandManager.InvalidateRequerySuggested();
		}

		private async void OnIssueMenuClick(object sender, RoutedEventArgs e)
		{
			await App.AppManager.Composite.Login();

			MainFrame.Navigate(new IssuePage(Context));
		}

		private void OnExitClick(object sender, RoutedEventArgs e)
		{
			App.CurrentApp.Shutdown();
		}
	}
}
