using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLandEF.Model.Tasks
{
    [Table("Tasks")]
    public class TaskModel
    {
        [Key]
        [Column("ListID")]
        public int ListID { get; set; }
        [Column("ListTittle")]
        public string ListTittle { get; set; }
        [Column("ListDescription")]
        public string ListDescription { get; set; }
        [Column("IsDone")]
        public bool IsDone { get; set; }
        [Column("ExpriteTime")]
        public DateTime ExpriteTime { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        public TaskModel(int listID, string listTittle, string listDescription, bool isDone, DateTime expriteTime, int userId)
        {
            ListID = listID;
            ListTittle = listTittle;
            ListDescription = listDescription;
            IsDone = isDone;
            ExpriteTime = expriteTime;
            UserId = userId;
        }
        public TaskModel()
        {
        }
    }
}
