using GraphQLandEF.DTO.Users;
using GraphQLandEF.Model.Tasks;
using GraphQLandEF.Services.Tasks;
using GraphQLandEF.Services.Users;
using HotChocolate;
using HotChocolate.Authorization;

namespace GraphQLandEF.Controllers
{
    public class Query
    {
        [Authorize]
        [GraphQLName("getTasksByUserId")]
        public async Task<List<TaskModel>> GetTasksByUserId([Service] TaskService taskService, int userId)
        {
            return await taskService.GetTasksByUserId(userId);

        }
     
    }
}
