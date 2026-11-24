namespace backend.DTOs;

public class RunReviewDto
{
    public string RunIdentifier { get; set; } = string.Empty;

    public DateTime RunDate { get; set; }

    public int TotalAllocatedUnits { get; set; }

    public int TotalGaps { get; set; }

    public int FilledCount { get; set; }

    public int PartiallyFilledCount { get; set; }

    public int UnfilledCount { get; set; }

    public List<GapReviewDto> Gaps { get; set; } = new();
}