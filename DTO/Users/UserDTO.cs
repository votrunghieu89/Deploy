namespace GraphQLandEF.DTO.UserDTo
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public UserDTO(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }
        public UserDTO()
        {
        }
    }
}
