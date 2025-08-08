using GraphQLandEF.DTO.Task;
using GraphQLandEF.Model.Tasks;

namespace GraphQLandEF.Repositories
{
    public interface ITaskRepository
    {
        Task<bool> AddTask(TaskDTO task);
        Task<bool> UpdateTask(TaskModel task);
        Task<bool> DeleteTask(int taskId);
        Task<List<TaskModel>> GetTaskById(int userID);
    }
}
