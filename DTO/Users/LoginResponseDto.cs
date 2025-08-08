namespace GraphQLandEF.DTO.Users
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public LoginResponseDto(string accessToken, int userId, string userName, string email)
        {
            AccessToken = accessToken;
            UserId = userId;
            UserName = userName;
            Email = email;
        }
        public LoginResponseDto() { }
    }
}
