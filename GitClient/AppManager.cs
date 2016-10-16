using GitClient.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GitClient
{
	public class AppManager
	{

		public AppManager()
		{
			LoadLogins();
		}

		private List<Login> Logins { get; set; }
		private void LoadLogins()
		{
			var logins = Properties.Settings.Default.Logins;
			Logins = JsonConvert.DeserializeObject<List<Login>>(logins) ?? new List<Login>();
		}

		public void AddLogin(Login login)
		{
			Logins.Add(login);
		}

		public void RemoveLogin(Login login)
		{
			Logins.Remove(login);
		}

		public bool Contains(Login login)
		{
			return Logins.Any(l => l.Username == login.Username && l.Provider == login.Provider);
		}

		public void SaveSettings()
		{
			Properties.Settings.Default.Logins = JsonConvert.SerializeObject(Logins);
			Properties.Settings.Default.Save();
		}

	}
}
