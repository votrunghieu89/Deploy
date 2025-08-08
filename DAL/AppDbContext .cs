using GraphQLandEF.Model.Tasks;
using GraphQLandEF.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace GraphQLandEF.DAL
{
    public class AppDbContext : DbContext
    {

        public DbSet<UserModel> users { get; set; }
        public DbSet<TaskModel> tasks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
