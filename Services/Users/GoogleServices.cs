using GraphQLandEF.DTO.UserDTo;
using GraphQLandEF.Model.Users;
using GraphQLandEF.Repositories;
using GraphQLandEF.Security;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ServiceStack.Auth;
using System.Net.Http.Headers;

namespace GraphQLandEF.Services.Users
{
    public class GoogleServices
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleSettingModel _googleConfig;
        private readonly AccesToken _accessToken;
        private readonly ILogger<GoogleServices> _logger;
        private readonly IUsersRepository _authRepository;

        public GoogleServices(
            HttpClient httpClient,
            IOptions<GoogleSettingModel> googleConfig,
            AccesToken accessToken,
            ILogger<GoogleServices> logger,
            IUsersRepository authRepository)
        {
            _httpClient = httpClient;
            _googleConfig = googleConfig.Value;
            _accessToken = accessToken;
            _logger = logger;
            _authRepository = authRepository;
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", _googleConfig.ClientId),
                new KeyValuePair<string, string>("client_secret", _googleConfig.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", _googleConfig.RedirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", requestContent);
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            return json["access_token"]?.ToString();
        }

        public async Task<JObject> GetGoogleUserInfoAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        public async Task<GoogleLoginResultModel> GoogleCallBack(string code)
        {
            var accessToken = await GetAccessTokenAsync(code);
            var userInfo = await GetGoogleUserInfoAsync(accessToken);
            if (userInfo == null)
            {
                _logger.LogError("Failed to retrieve user information from Google.");
                return null;
            }
            string email = userInfo["email"]?.ToString();
            string userName = userInfo["name"]?.ToString();
            UserModel existingUser = await _authRepository.IsEmailExists(email);
            if (existingUser == null)
            {
                UserDTO newUser = new UserDTO
                {
                    UserName = userName,
                    Email = email,
                    Password = ""
                };
                var registrationResult = await _authRepository.Register(newUser);
                if (!registrationResult)
                {
                    _logger.LogError("Failed to register new user: {UserName}", userName);
                    return null;
                }
                UserModel userModel = await _authRepository.GetUserByUserName(newUser.UserName);
                Console.WriteLine($"User registered: {userModel.UserName} {userModel.UserId} {userModel.Email}");
                existingUser = userModel;
            }
            var jwt = await _accessToken.GenerateAccessToken(existingUser);
            GoogleLoginResultModel result = new GoogleLoginResultModel
            {
                UserId = existingUser.UserId,
                AccessToken = jwt,
                UserName = existingUser.UserName,
                Email = existingUser.Email
            };
            return result;
        }
    }
}
