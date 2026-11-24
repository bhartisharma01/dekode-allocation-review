namespace backend.DTOs;

public class RunHistoryDto
{
    public string RunIdentifier { get; set; } = string.Empty;

    public DateTime RunDate { get; set; }

    public int TotalAllocatedUnits { get; set; }
}