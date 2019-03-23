using System.Threading.Tasks;
using Application.API.Models;

namespace Application.API.Data {
    public interface IAuthRepository {
        Task<User> Register (User User, string password);
        Task<User> Login (string username, string password);
        Task<bool> UserExists (string username);
    }
}