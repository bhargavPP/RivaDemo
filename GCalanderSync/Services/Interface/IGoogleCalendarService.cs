using Google.Apis.Calendar.v3;

namespace GCalanderSync.Services.Interface
{
    public interface IGoogleCalendarService
    {
        Task<CalendarService> GetCalendarServiceAsync();
        Task<IList<string>> GetUpcomingEventsAsync(string accessToken, string refreshToken, string ClientId,string ClientSecret);
    }
}
