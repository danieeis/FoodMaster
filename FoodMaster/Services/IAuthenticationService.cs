using System.Threading.Tasks;

namespace FoodMaster.Services
{
    public interface IAuthenticationService
    {
        bool IsSignIn();
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> RegisterWithEmailPassword(string email, string password);
        bool SignOut();
    }
}
