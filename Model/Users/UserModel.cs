using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLandEF.Model.Users
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("Email")]
        public string Email { get; set; }

        public UserModel(int userId, string userName, string password, string email)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
        }

        public UserModel()
        {
        }
    }
}
