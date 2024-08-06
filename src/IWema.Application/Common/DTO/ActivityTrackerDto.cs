namespace IWema.Application.Common.DTO;

public class ActivityTrackerDto
{
    public string Feature { get; set; }=string.Empty;
    public DateTime Date { get; set; }
    public int VisitCount { get; set; }
}
