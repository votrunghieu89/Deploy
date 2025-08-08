using GraphQLandEF.DTO.UserDTo;
using GraphQLandEF.Model.Users;
using GraphQLandEF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GraphQLandEF.DAL.Users
{
    public class UsersDAL : IUsersRepository
    {
        private readonly AppDbContext _appDbContext;
        public UsersDAL(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserModel> GetUserByUserName(string username)
        {
           bool checkConnection = await _appDbContext.Database.CanConnectAsync();
            if (!checkConnection)
            {
                return null; 
            }
            UserModel? user = await _appDbContext.users.Where(u => u.UserName == username)
                .Select(u => new UserModel
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                }).FirstOrDefaultAsync(); 

            if (user != null)
            {
                return user; 
            }
            else
            {
                return null; 
            }
        }

        public async Task<UserModel> IsEmailExists(string email)
        {
            bool checkConnection = await _appDbContext.Database.CanConnectAsync();
            if (!checkConnection)
            {
                return null;
            }
            UserModel? emailExists = await _appDbContext.users.FirstOrDefaultAsync(u => u.Email == email); // tìm kiếm theo điều kiền
           
            if (emailExists != null)
            {
                return emailExists;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsUserExists(string username)
        {
            bool checkConnection = await _appDbContext.Database.CanConnectAsync();
            if (!checkConnection)
            {
                return false;
            }
            bool userExists = await _appDbContext.users.AnyAsync(u => u.UserName == username);
            if (userExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            bool checkConnection = await _appDbContext.Database.CanConnectAsync();
            if (!checkConnection)
            {
                return false;
            }
            bool isLogin = await _appDbContext.users.AnyAsync(u => u.UserName == username && u.Password == password);
            if (isLogin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Register(UserDTO user)
        {
            bool checkConnection = await _appDbContext.Database.CanConnectAsync();
            if (!checkConnection)
            {
                Console.WriteLine("Database connection failed.");
                return false;
            }
            UserModel newUser = new UserModel
            {
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            };
            await _appDbContext.users.AddAsync(newUser);
            int result = await _appDbContext.SaveChangesAsync();
            if (result > 0)
            {
                Console.WriteLine("User registered successfully.");
                return true; // Đăng ký thành công
            }
            else
            {
                Console.WriteLine("User registration failed.");
                return false; // Đăng ký thất bại
            }
        }
    }
}
