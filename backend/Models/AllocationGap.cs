namespace backend.Models;

public class AllocationGap
{
    public int Id { get; set; }

    public string GapIdentifier { get; set; } = string.Empty;

    public string Store { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string ProductType { get; set; } = string.Empty;

    public decimal OriginalShortfall { get; set; }

    public int RunId { get; set; }

    public AllocationRun Run { get; set; } = null!;

    public ICollection<BatchAllocation> BatchAllocations { get; set; }
        = new List<BatchAllocation>();
}