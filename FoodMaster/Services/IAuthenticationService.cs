using System.Threading.Tasks;
using FoodMaster.Models;

namespace FoodMaster.Services
{
    public interface IAuthenticationService
    {
        bool IsSignIn();
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> RegisterWithEmailPassword(string name, string email, string password);
        Task UpdateName(string names);
        bool SignOut();
        User GetUserAsync();
    }
}
