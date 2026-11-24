namespace backend.Models;

public class BatchAllocation
{
    public int Id { get; set; }

    public string BatchIdentifier { get; set; } = string.Empty;

    public int AllocatedUnits { get; set; }

    public int GapId { get; set; }

    public AllocationGap Gap { get; set; } = null!;
}