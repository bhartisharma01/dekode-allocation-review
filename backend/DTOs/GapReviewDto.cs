namespace backend.DTOs;

public class GapReviewDto
{
    public string GapIdentifier { get; set; } = string.Empty;

    public string Store { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string ProductType { get; set; } = string.Empty;

    public decimal OriginalShortfall { get; set; }

    public int AllocatedUnits { get; set; }

    public int RemainingUnits { get; set; }

    public string Status { get; set; } = string.Empty;
}