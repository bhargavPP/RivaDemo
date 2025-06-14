using GCalanderSync.Services.Interface;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GCalanderSync.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly IConfiguration _configuration;

        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "GCalanderSync";

        public GoogleCalendarService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<CalendarService> GetCalendarServiceAsync()
        {
            var clientId = _configuration["GoogleAPI:ClientId"];
            var clientSecret = _configuration["GoogleAPI:ClientSecret"];

            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    }, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true));

                return new CalendarService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
            }
        }

        public async Task<IList<string>> GetUpcomingEventsAsync(string accessToken, string refreshToken, string clientId, string clientSecret)
        {
            var tokenResponse = new Google.Apis.Auth.OAuth2.Responses.TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { CalendarService.Scope.CalendarReadonly }
            });

            var credential = new UserCredential(flow, "user", tokenResponse);

            try
            {
                // Refresh access token if expired
                bool refreshed = await credential.RefreshTokenAsync(CancellationToken.None);

                if (!refreshed)
                    throw new Exception("Unable to refresh the access token.");
            }
            catch (Google.Apis.Auth.OAuth2.Responses.TokenResponseException ex)
            {
                // Refresh token is invalid or expired
                throw new Exception("Refresh token is invalid or expired. Please re-authorize.", ex);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            // Step 1: List all calendars user has access to
            var calendarList = await service.CalendarList.List().ExecuteAsync();

            List<string> allEventSummaries = new();

            foreach (var calendar in calendarList.Items)
            {
                // Step 2: For each calendar, fetch upcoming events
                var request = service.Events.List(calendar.Id);
                request.TimeMin = DateTime.UtcNow;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;  // You can adjust or paginate
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = await request.ExecuteAsync();

                if (events.Items != null)
                {
                    foreach (var ev in events.Items)
                    {
                        var when = ev.Start?.DateTime?.ToString() ?? ev.Start?.Date;
                        allEventSummaries.Add($"{calendar.Summary}: {when} - {ev.Summary}");
                        Console.WriteLine($"{calendar.Summary}: {when} - {ev.Summary}");
                    }
                }
            }

            return allEventSummaries;
        }


        public async Task<CalendarService> GetCalendarServiceAsync(string accessToken)
        {
            var initializer = new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(accessToken),
                ApplicationName = ApplicationName
            };

            return new CalendarService(initializer);
        }

    }
}
