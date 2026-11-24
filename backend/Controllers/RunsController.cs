
using backend.Data;
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace backend.Controllers;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/runs")]
public class RunsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly AllocationCalculationService _calculationService;

    public RunsController(
        ApplicationDbContext db,
        AllocationCalculationService calculationService)
    {
        _db = db;
        _calculationService = calculationService;
    }

    // GET: api/runs?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResult<RunHistoryDto>>> GetRunHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Validate page number.
        if (page < 1)
        {
            return BadRequest(new
            {
                message = "Page must be greater than or equal to 1."
            });
        }

        // Validate page size (maximum 50 records).
        if (pageSize < 1 || pageSize > 50)
        {
            return BadRequest(new
            {
                message = "Page size must be between 1 and 50."
            });
        }

        // Get total number of runs.
        var totalCount = await _db.AllocationRuns.CountAsync();

        // Fetch only the requested page.
        var items = await _db.AllocationRuns
            .AsNoTracking()
            .OrderByDescending(run => run.RunDate)
            .ThenByDescending(run => run.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(run => new RunHistoryDto
            {
                RunIdentifier = run.RunIdentifier,
                RunDate = run.RunDate,

                TotalAllocatedUnits = _db.BatchAllocations
                    .Where(batch => batch.Gap.RunId == run.Id)
                    .Sum(batch => (int?)batch.AllocatedUnits) ?? 0
            })
            .ToListAsync();

        // Prepare paginated response.
        var response = new PagedResult<RunHistoryDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize)
        };

        return Ok(response);
    }

    // GET: api/runs/R-102
    // GET: api/runs/R-102?exceptionsOnly=true
    [HttpGet("{runIdentifier}")]
    public async Task<ActionResult<RunReviewDto>> GetRunReview(
        string runIdentifier,
        [FromQuery] bool exceptionsOnly = false)
    {
        // Fetch run, gaps, and their batch allocations.
        var run = await _db.AllocationRuns
            .AsNoTracking()
            .Include(r => r.Gaps)
                .ThenInclude(g => g.BatchAllocations)
            .FirstOrDefaultAsync(
                r => r.RunIdentifier == runIdentifier);

        // Return 404 if run doesn't exist.
        if (run == null)
        {
            return NotFound(new
            {
                message = $"Run '{runIdentifier}' was not found."
            });
        }

        // Calculate details for every gap.
        var allGapReviews = run.Gaps
            .Select(gap => _calculationService.CalculateGap(gap))
            .ToList();

        // Build complete run summary.
        var response = new RunReviewDto
        {
            RunIdentifier = run.RunIdentifier,
            RunDate = run.RunDate,

            TotalAllocatedUnits = allGapReviews
                .Sum(gap => gap.AllocatedUnits),

            TotalGaps = allGapReviews.Count,

            FilledCount = allGapReviews
                .Count(gap => gap.Status == "Filled"),

            PartiallyFilledCount = allGapReviews
                .Count(gap => gap.Status == "Partially filled"),

            UnfilledCount = allGapReviews
                .Count(gap => gap.Status == "Unfilled"),

            // Apply exceptions filter only to gap rows.
            Gaps = exceptionsOnly
                ? allGapReviews
                    .Where(gap =>
                        gap.Status == "Partially filled" ||
                        gap.Status == "Unfilled")
                    .ToList()
                : allGapReviews
        };

        return Ok(response);
    }
}
