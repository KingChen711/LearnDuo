using TutorDemand.Data.Enums;

namespace TutorDemand.RazorWebApp.Models
{
    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
    }
}
