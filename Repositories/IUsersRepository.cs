using GraphQLandEF.DTO.UserDTo;
using GraphQLandEF.Model.Users;

namespace GraphQLandEF.Repositories
{
    public interface IUsersRepository
    {
        Task<bool> Register(UserDTO user);
        Task<bool> Login(string username, string password);
        Task<bool> IsUserExists(string username);
        Task<UserModel> IsEmailExists(string email);
        Task<UserModel> GetUserByUserName(string username);

    }
}
