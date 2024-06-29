namespace TutorDemand.Data.Dtos.TeachingSchedule;

public class QueryTeachingScheduleDto
{
    private const int MaxPageSize = 50;
    private int _pageSize = 5;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, MaxPageSize);
    }

    public int PageNumber { get; set; } = 1;
    public string? SearchSubject { get; set; }
    public string? SearchTutor { get; set; }
    public string? SearchSlot { get; set; }
}