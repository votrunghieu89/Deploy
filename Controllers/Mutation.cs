using GraphQLandEF.DTO.Task;
using GraphQLandEF.DTO.UserDTo;
using GraphQLandEF.DTO.Users;
using GraphQLandEF.Model.Tasks;
using GraphQLandEF.Model.Users;
using GraphQLandEF.Services.Tasks;
using GraphQLandEF.Services.Users;
using HotChocolate;
using HotChocolate.Authorization;

namespace GraphQLandEF.Controllers
{
    public class Mutation
    {
        [Authorize]
        [GraphQLName("createTask")]
        public async Task<string> CreateTask([Service] TaskService taskService, TaskDTO task)
        {
            return await taskService.CreateTask(task);
        }
        [Authorize]
        [GraphQLName("removeTask")]
        public async Task<string> RemoveTask([Service] TaskService taskService, int taskId)
        {
            return await taskService.RemoveTask(taskId);
        }
        [Authorize]
        [GraphQLName("updateTask")]
        public async Task<string> UpdateTask([Service] TaskService taskService, TaskModel task)
        {
            return await taskService.UpdateTask(task);
        }
    
        [GraphQLName("register")]
        public async Task<string> RegisterUser([Service] AuthService authService, UserDTO user)
        {
            return await authService.Register(user);
        }

        [GraphQLName("login")]
        public async Task<LoginResponseDto> LoginUser([Service] AuthService authService, string username, string password)
        {
            return await authService.Login(username, password);
        }
    }
}

