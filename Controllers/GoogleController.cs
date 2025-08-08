using GraphQLandEF.Model.Users;
using GraphQLandEF.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceStack;

namespace GraphQLandEF.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class GoogleController : ControllerBase
    {
        private readonly GoogleServices _googleService;
        private readonly ILogger<GoogleController> _logger;
        private readonly GoogleSettingModel _googleConfig;

        public GoogleController(
            GoogleServices googleService,
            ILogger<GoogleController> logger,
            IOptions<GoogleSettingModel> googleConfig)
        {
            _googleService = googleService;
            _logger = logger;
            _googleConfig = googleConfig.Value;
        }
        [AllowAnonymous]
        [HttpGet("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={_googleConfig.ClientId}&redirect_uri={_googleConfig.RedirectUri}&response_type=code&scope=email%20profile&access_type=offline";
            Console.WriteLine(redirectUrl);
            return Redirect(redirectUrl);
        }
        [AllowAnonymous]
        [HttpGet("callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCallback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("Google callback received without code.");
                return BadRequest("Invalid callback request.");
            }
            try
            {
                var result = await _googleService.GoogleCallBack(code);
                if (result == null)
                {
                    _logger.LogError("Failed to retrieve user information from Google.");
                    return BadRequest("Failed to retrieve user information.");
                }
                var accessToken = result.AccessToken ?? "";

                // Trả về HTML nhỏ để FE nhận messages
                var html = $@"
            <script>
                window.opener.postMessage({{ accessToken: '{accessToken}' }}, 'http://localhost:3000');
                window.close();
            </script>
            Đăng nhập thành công, vui lòng đợi...
        ";

                return Content(html, "text/html");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google callback processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

    }
}
