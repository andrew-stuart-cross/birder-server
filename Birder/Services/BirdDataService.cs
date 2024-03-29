using Microsoft.EntityFrameworkCore;

namespace Birder.Services;

public interface IBirdDataService
{
    Task<BirdsListDto> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter);
    Task<IReadOnlyList<BirdSummaryDto>> GetBirdsDropDownListAsync();
    Task<BirdDetailDto> GetBirdAsync(int id);
}

public class BirdDataService : IBirdDataService
{
    private readonly ApplicationDbContext _dbContext;

    public BirdDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BirdsListDto> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter)
    {
        var query = _dbContext.Birds
            .MapBirdToBirdSummaryDto()
            .AsNoTracking()
            .AsQueryable();

        if (speciesFilter == BirderStatus.Common)
        {
            query = query.Where(bs => bs.BirderStatus == BirderStatus.Common);
        }

        query = query.OrderBy(s => s.BirderStatus)
                     .ThenBy(n => n.EnglishName);

        var result = new BirdsListDto();

        result.TotalItems = await query.CountAsync();

        query = query.ApplyPaging(pageIndex, pageSize);

        result.Items = await query.ToListAsync();

        return result;
    }

    public async Task<IReadOnlyList<BirdSummaryDto>> GetBirdsDropDownListAsync()
    {
        var query = _dbContext.Birds
            .MapBirdToBirdSummaryDto()
            .AsNoTracking()
            .AsQueryable();

        query = query.OrderBy(s => s.BirderStatus)
                     .ThenBy(n => n.EnglishName);

        return await query.ToListAsync();
    }

    public async Task<BirdDetailDto> GetBirdAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException(nameof(id));
        }

        var query = _dbContext.Birds
            .MapBirdToBirdDetailDto()
            .AsNoTracking()
            .Where(b => b.BirdId == id);

        return await query.FirstOrDefaultAsync();
    }
}