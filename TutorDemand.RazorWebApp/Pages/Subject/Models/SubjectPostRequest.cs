namespace TutorDemand.RazorWebApp.Pages.Subject.Models
{
    public class SubjectPostRequest
    {
        public string SearchValue { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public int PageIndex { get; set; }
    }
}