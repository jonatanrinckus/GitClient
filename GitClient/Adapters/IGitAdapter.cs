using GitClient.Models;
using System.Threading.Tasks;

namespace GitClient.Adapters
{
	public interface IGitAdapter
	{
		Task<bool> Login(Login login);
		User GetUserInfo();
	}
}
