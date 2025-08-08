namespace GraphQLandEF.Model.Users
{
    public class GoogleLoginResultModel
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public GoogleLoginResultModel(string accessToken, int userID, string email, string userName)
        {
            AccessToken = accessToken;
            UserId = userID;
            Email = email;
            UserName = userName;
        }
        public GoogleLoginResultModel()
        {
        }
    }
}
