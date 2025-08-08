using GraphQLandEF.DTO.Task;
using GraphQLandEF.Model.Tasks;
using GraphQLandEF.Repositories;

namespace GraphQLandEF.Services.Tasks
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<string> CreateTask(TaskDTO task)
        {
            if (task == null)
            {
                return "Task data is null.";
            }
            if (string.IsNullOrEmpty(task.ListTittle) || string.IsNullOrEmpty(task.ListDescription))
            {
                return "Title and description are required.";
            }
            bool isTaskCreated = await _taskRepository.AddTask(task);
            Console.WriteLine($"Task creation result: {isTaskCreated}");
            if (isTaskCreated)
            {
                return "Task created successfully.";
            }
            else
            {
                return "Failed to create task.";
            }
        }
        public async Task<string> RemoveTask(int taskId)
        {
            if (taskId <= 0)
            {
                return "Invalid task ID.";
            }
            bool isTaskDeleted = await _taskRepository.DeleteTask(taskId);
            if (isTaskDeleted)
            {
                return "Task deleted successfully.";
            }
            else
            {
                return "Failed to delete task.";
            }
        }
        public async Task<List<TaskModel>> GetTasksByUserId(int userId)
        {
            if (userId <= 0)
            {
                return null; // or throw an exception
            }
            List<TaskModel> tasks = await _taskRepository.GetTaskById(userId);
            if (tasks != null && tasks.Count > 0)
            {
                return tasks;
            }
            else
            {
                return new List<TaskModel>(); // return empty list if no tasks found
            }
        }
        public async Task<string> UpdateTask(TaskModel task)
        {
            if (task == null || task.ListID <= 0)
            {
                return "Invalid task data.";
            }
            bool isTaskUpdated = await _taskRepository.UpdateTask(task);
            if (isTaskUpdated)
            {
                return "Task updated successfully.";
            }
            else
            {
                return "Failed to update task.";
            }
        }
    }
}
