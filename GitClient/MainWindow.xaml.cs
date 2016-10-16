using GitClient.Adapters;
using GitClient.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GitClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel Context => ((MainViewModel) DataContext);

		public MainWindow()
        {
            InitializeComponent();

			((MainViewModel)DataContext).Status = "Starting...";

			//  DispatcherTimer setup
			var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
	
			Context.Status = "Welcome";

		}

        

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            Context.Clock = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second.ToString("D2")}";

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

	    private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
	    {
			IGitAdapter client = new GitHubAdapter();
		    if (await client.Login(LoginTextBox.Text, PasswordBox.Password))
		    {
			    var user = client.GetUserInfo();
				Context.Status = $"Logged as {user.Name}";
			}
		    else
		    {
			    if (App.CurrentApp.HasError)
			    {
				    while (App.CurrentApp.HasError)
				    {
					    var error = App.CurrentApp.Errors.FirstOrDefault();
					    Context.Status = error;
					    App.CurrentApp.Errors.Remove(error);
					    await Task.Delay(1000);
				    }
			    }
			    else
			    {
					Context.Status = "Login error";
				}
		    }
	    }
    }
}
