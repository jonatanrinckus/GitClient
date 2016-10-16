using GitClient.Models;
using System.Threading.Tasks;

namespace GitClient.Adapters
{
	public interface IGitAdapter
	{
		Task<bool> Login(string login, string password);
		User GetUserInfo();
	}
}
