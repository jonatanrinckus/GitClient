using GitClient.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GitClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static App CurrentApp;
		public static AppManager AppManager;
		public List<string> Errors { get; }
		public bool HasError => Errors.Any();

		public App()
		{
			CurrentApp = this;
			Errors = new List<string>();
			AppManager = new AppManager();
			Login();
		}

		private void Login()
		{
			//Disable shutdown when the dialog closes
			Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

			var loginWindow = new LoginWindow();

			if (loginWindow.ShowDialog() == true)
			{
				var mainWindow = new MainWindow();
				//Re-enable normal shutdown mode.
				Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
				Current.MainWindow = mainWindow;
				mainWindow.Show();
			}
			else
			{
				Current.Shutdown();
			}
		}


		private void OnExit(object sender, ExitEventArgs e)
		{
			AppManager.SaveSettings();
		}
	}
}
