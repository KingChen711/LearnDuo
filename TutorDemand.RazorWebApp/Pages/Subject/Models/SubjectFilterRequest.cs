namespace TutorDemand.RazorWebApp.Pages.Subject.Models
{
    public class SubjectFilterRequest
    {
        public string SubjectName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}
