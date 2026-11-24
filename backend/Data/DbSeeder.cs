
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // Avoid inserting the same runs more than once.
        if (await db.AllocationRuns.AnyAsync())
            return;

        var run102 = new AllocationRun
        {
            RunIdentifier = "R-102",
            RunDate = new DateTime(2026, 9, 14),
            Gaps = new List<AllocationGap>
            {
                new()
                {
                    GapIdentifier = "G-01",
                    Store = "Store A",
                    Brand = "Brand X",
                    ProductType = "Frames",
                    OriginalShortfall = 5,
                    BatchAllocations = new List<BatchAllocation>
                    {
                        new()
                        {
                            BatchIdentifier = "B-401",
                            AllocatedUnits = 2
                        },
                        new()
                        {
                            BatchIdentifier = "B-402",
                            AllocatedUnits = 1
                        }
                    }
                },
                new()
                {
                    GapIdentifier = "G-02",
                    Store = "Store B",
                    Brand = "Brand X",
                    ProductType = "Frames",
                    OriginalShortfall = 2,
                    BatchAllocations = new List<BatchAllocation>
                    {
                        new()
                        {
                            BatchIdentifier = "B-501",
                            AllocatedUnits = 2
                        }
                    }
                },
                new()
                {
                    GapIdentifier = "G-03",
                    Store = "Store C",
                    Brand = "Brand Y",
                    ProductType = "Sunglasses",
                    OriginalShortfall = 4
                }
            }
        };

        var run101 = new AllocationRun
        {
            RunIdentifier = "R-101",
            RunDate = new DateTime(2026, 9, 13),
            Gaps = new List<AllocationGap>
            {
                new()
                {
                    GapIdentifier = "G-01",
                    Store = "Store D",
                    Brand = "Brand Z",
                    ProductType = "Frames",
                    OriginalShortfall = 1,
                    BatchAllocations = new List<BatchAllocation>
                    {
                        new()
                        {
                            BatchIdentifier = "B-601",
                            AllocatedUnits = 1
                        }
                    }
                }
            }
        };

        db.AllocationRuns.AddRange(run102, run101);

        await db.SaveChangesAsync();
    }
}
