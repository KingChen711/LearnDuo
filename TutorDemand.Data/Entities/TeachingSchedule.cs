using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorDemand.Data.Entities;

public class TeachingSchedule
{
    public int Id { get; set; }

    public Guid TeachingScheduleId { get; set; }

    public Guid TutorId { get; set; }

    public Guid SubjectId { get; set; }

    public Guid SlotId { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Mã phòng google meet tối đa 50 kí tự")]
    [DisplayName("Mã phòng google meet")]
    public string MeetRoomCode { get; set; } = null!;

    [Required]
    [MaxLength(50, ErrorMessage = "Mật khẩu phòng google meet tối đa 50 kí tự")]
    [DisplayName("Mật khẩu phòng google meet")]
    public string RoomPassword { get; set; } = null!;

    [DisplayName("Các thứ học trong tuần")]
    [MaxLength(100, ErrorMessage = "Các thứ học trong tuần tối đa 100 kí tự")]
    public string LearnDays { get; set; } = null!; //Example: "Monday,Wednesday,Friday"

    [Range(0, int.MaxValue, ErrorMessage = "Giá khóa học phải lớn hơn 0")]
    [DisplayName("Giá khóa học")]
    public int PaidPrice { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    //navigator
    public Subject Subject { get; set; } = null!;
    public Tutor Tutor { get; set; } = null!;
    public Slot Slot { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = [];
}
