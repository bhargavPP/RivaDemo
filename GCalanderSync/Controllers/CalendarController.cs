using GCalanderSync.Services.Interface;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GCalanderSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGoogleCalendarService _googleCalendarService;

        // For demo, storing tokens in-memory (replace with DB or persistent store!)
        private static string _accessToken = null;
        private static string _refreshToken = null;

        public CalendarController(IConfiguration configuration, IGoogleCalendarService googleCalendarService)
        {
            _configuration = configuration;
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet("authorize")]
        public IActionResult Authorize()
        {
            var clientId = _configuration["GoogleAPI:ClientId"];
            var redirectUri = "https://localhost:7076/api/calendar/oauth2callback"; // Must match Google Console exactly
            var scope = "https://www.googleapis.com/auth/calendar.readonly";

            var authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth" +
                $"?client_id={clientId}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&response_type=code" +
                $"&scope={Uri.EscapeDataString(scope)}" +
                $"&access_type=offline" +
                $"&prompt=consent";

            return Redirect(authorizationUrl); // Important: full browser redirect
        }

        [HttpGet("oauth2callback")]
        public async Task<IActionResult> OAuth2Callback([FromQuery] string? code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error))
                return BadRequest($"OAuth error: {error}");

            if (string.IsNullOrEmpty(code))
                return BadRequest(new { error = "Authorization code not found." });

            var clientId = _configuration["GoogleAPI:ClientId"];
            var clientSecret = _configuration["GoogleAPI:ClientSecret"];
            var redirectUri = "https://localhost:7076/api/calendar/oauth2callback";

            using var httpClient = new HttpClient();

            var requestData = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("code", code),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("client_secret", clientSecret),
        new KeyValuePair<string, string>("redirect_uri", redirectUri),
        new KeyValuePair<string, string>("grant_type", "authorization_code")
    });

            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", requestData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest($"Token exchange failed: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<Google.Apis.Auth.OAuth2.Responses.TokenResponse>(json);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;

            try
            {
                var events = await _googleCalendarService.GetUpcomingEventsAsync(_accessToken, _refreshToken, _configuration["GoogleAPI:ClientId"], _configuration["GoogleAPI:ClientSecret"]);
                return Ok(new { message = "OAuth successful!", events });
            }
            catch (Exception ex)
            {
                // Clear tokens
                _accessToken = null;
                _refreshToken = null;

                return Unauthorized(new { message = "Your session expired. Please authorize again.", reauthorizeUrl = "/api/calendar/authorize" });
            }

             
        }


        //[HttpGet("events")]
        //public async Task<IActionResult> GetEvents()
        //{
        //    if (string.IsNullOrEmpty(_accessToken))
        //        return Unauthorized("User is not authorized. Please call /api/calendar/authorize first.");

        //    var events = await _googleCalendarService.GetUpcomingEventsAsync(_accessToken);

        //    return Ok(events);
        //}
    }
}
