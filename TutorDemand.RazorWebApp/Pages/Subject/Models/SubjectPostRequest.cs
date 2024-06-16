using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TutorDemand.RazorWebApp.Pages.Subject.Models
{
    public class SubjectPostRequest
    {
        public string SearchValue { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public int PageIndex { get; set; }

        public string? SubjectCode { get; set; } = null!;
        public string? SubjectName { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must greater than 0")]
        public decimal? Price { get; set; }
    }
}