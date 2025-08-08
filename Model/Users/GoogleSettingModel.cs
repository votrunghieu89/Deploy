namespace GraphQLandEF.Model.Users
{
    public class GoogleSettingModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string RedirectUri { get; set; }

        public GoogleSettingModel(string clientId, string clientSecret, string redirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
        }
        public GoogleSettingModel()
        {

        }
    }
}
