using System.Threading.Tasks;

namespace FoodMaster.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginWithEmailPassword(string email, string password);
    
    }
}
