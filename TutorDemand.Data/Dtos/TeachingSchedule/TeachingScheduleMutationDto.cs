using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Dtos.TeachingSchedule
{
    public record TeachingScheduleMutationDto
    {
        [Required] public Guid TutorId { get; set; }

        [Required] public Guid SubjectId { get; set; }

        [Required] public Guid SlotId { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Meet Room Code")]
        public string MeetRoomCode { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [DisplayName("Room Password")]
        public string RoomPassword { get; set; } = null!;

        [DisplayName("Learn Days")]
        public string? LearnDays { get; set; } //Example: "Monday,Wednesday,Friday"

        [Required]
        [DisplayName("Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateOnly EndDate { get; set; }
    }
}