using backend.DTOs;
using backend.Models;

namespace backend.Services;

public class AllocationCalculationService
{
    public GapReviewDto CalculateGap(AllocationGap gap)
    {
        // Convert decimal shortfall into whole units by rounding up.
        int requiredUnits = (int)Math.Ceiling(gap.OriginalShortfall);

        // Sum allocations from all batches belonging to this gap.
        int allocatedUnits = gap.BatchAllocations
            .Sum(batch => batch.AllocatedUnits);

        // Remaining units cannot be negative.
        int remainingUnits = Math.Max(
            0,
            requiredUnits - allocatedUnits
        );

        // Determine the gap status.
        string status;

        if (remainingUnits == 0)
        {
            status = "Filled";
        }
        else if (allocatedUnits == 0)
        {
            status = "Unfilled";
        }
        else
        {
            status = "Partially filled";
        }

        return new GapReviewDto
        {
            GapIdentifier = gap.GapIdentifier,
            Store = gap.Store,
            Brand = gap.Brand,
            ProductType = gap.ProductType,
            OriginalShortfall = gap.OriginalShortfall,
            AllocatedUnits = allocatedUnits,
            RemainingUnits = remainingUnits,
            Status = status
        };
    }
}