using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLandEF.DTO.Task
{
    public class TaskDTO
    {
        public string ListTittle { get; set; }
  
        public string ListDescription { get; set; }
     
        public bool IsDone { get; set; }

        public DateTime ExpriteTime { get; set; }
   
        public int UserId { get; set; }

        public TaskDTO(string listTittle, string listDescription, bool isDone, DateTime expriteTime, int userId)
        {
            ListTittle = listTittle;
            ListDescription = listDescription;
            IsDone = isDone;
            ExpriteTime = expriteTime;
            UserId = userId;
        }
        public TaskDTO()
        {
        }
    }
}
