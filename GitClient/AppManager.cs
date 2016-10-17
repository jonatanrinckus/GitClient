using GitClient.Helpers;
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
			Providers = new List<string>()
			{
				"GitHub", "GitLab"
			};

			LoadLogins();
		}

		public List<Login> Logins { get; private set; }
		public List<string> Providers { get; set; }
		private void LoadLogins()
		{
			var logins = Properties.Settings.Default.Logins;
			Logins = JsonConvert.DeserializeObject<List<Login>>(logins) ?? new List<Login>();

			var psw = new PasswordEncoder();
			foreach (var login in Logins)
			{
				psw.EncryptedPassword = login.Password;
				login.Password = psw.DecryptWithByteArray();
			}
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

			var pwd = new PasswordEncoder();

			var logins = Logins;

			foreach (var login in logins)
			{
				login.Password = pwd.EncryptWithByteArray(login.Password);
			}

			Properties.Settings.Default.Logins = JsonConvert.SerializeObject(logins);
			Properties.Settings.Default.Save();
		}

	}
}
