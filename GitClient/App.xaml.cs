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
	    public List<string> Errors { get; set; }
		public bool HasError => Errors.Any();

		public App()
		{
			CurrentApp = this;
			Errors = new List<string>();
		}
    }
}
