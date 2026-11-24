namespace backend.Models;

public class AllocationRun
{
    public int Id { get; set; }

    public string RunIdentifier { get; set; } = string.Empty;

    public DateTime RunDate { get; set; }

    public ICollection<AllocationGap> Gaps { get; set; }
        = new List<AllocationGap>();
}