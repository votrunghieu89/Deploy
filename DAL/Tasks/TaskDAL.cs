using GraphQLandEF.DTO.Task;
using GraphQLandEF.Model.Tasks;
using GraphQLandEF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GraphQLandEF.DAL.Tasks
{
    public class TaskDAL : ITaskRepository
    {
        private readonly AppDbContext _appDbContext;
        public TaskDAL(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> AddTask(TaskDTO task)
        {
            bool isconecction = await _appDbContext.Database.CanConnectAsync();
            if (!isconecction)
            {
                return false;
            }
            TaskModel newTask = new TaskModel
            {
                ListTittle = task.ListTittle,
                ListDescription = task.ListDescription,
                IsDone = false,
                ExpriteTime = task.ExpriteTime,
                UserId = task.UserId
            };
            await _appDbContext.tasks.AddAsync(newTask);
            int result = await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Task creation result: {result}");
            if (result >0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteTask(int taskId)
        {
            bool isconecction = await _appDbContext.Database.CanConnectAsync();
            if (!isconecction)
            {
                return false;
            }
            await _appDbContext.tasks.Where(t => t.ListID == taskId).ExecuteDeleteAsync();
            int result = await _appDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<TaskModel>> GetTaskById(int userID)
        {
            bool isconecction = await _appDbContext.Database.CanConnectAsync();
            if (!isconecction)
            {
                return null;
            }
            List<TaskModel> tasks = await _appDbContext.tasks.Where(t => t.UserId == userID).ToListAsync();
            if (tasks != null && tasks.Count > 0)
            {
                return tasks;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateTask(TaskModel task)
        {
            bool isconecction = await _appDbContext.Database.CanConnectAsync();
            if (!isconecction)
            {
                return false;
            }
            await _appDbContext.tasks.Where(t => t.ListID == task.ListID).ExecuteUpdateAsync(
            set => set.SetProperty(t => t.ListTittle, task.ListTittle)
                      .SetProperty(t => t.ListDescription, task.ListDescription)
                      .SetProperty(t => t.IsDone, task.IsDone)
                      .SetProperty(t => t.ExpriteTime, task.ExpriteTime)
                      .SetProperty(t => t.UserId, task.UserId)
            );
            int result = await _appDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
