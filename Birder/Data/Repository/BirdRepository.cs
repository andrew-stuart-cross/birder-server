﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Birder.Data.Repository;
public interface IBirdRepository
{
    Task<IEnumerable<Bird>> GetBirdsDdlAsync();
    Task<Bird> GetBirdAsync(int id);
    Task<QueryResult<Bird>> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter);
}

public class BirdRepository : IBirdRepository
{
    private readonly ApplicationDbContext _dbContext;
    public BirdRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    // -----> BirdDetailDto
    public Task<Bird> GetBirdAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException(nameof(id));
        }

        return (from b in _dbContext.Birds
                .Include(cs => cs.BirdConservationStatus) // THIS IS BAD!  It include an array of all birds linked to the specific conserve status!!!
                                                          //.MapBirdToBirdDetailDto()
                where (b.BirdId == id)
                select b).FirstOrDefaultAsync();
    }

    // -----> BirdSummaryDto
    public async Task<IEnumerable<Bird>> GetBirdsDdlAsync()
    {
        return await _dbContext.Birds
                .Include(cs => cs.BirdConservationStatus) // THIS IS BAD!
                .OrderBy(ob => ob.BirderStatus)
                .ThenBy(a => a.EnglishName)
                .AsNoTracking()
                .ToListAsync();
    }



    // -------> BirdSummaryDto
    public async Task<QueryResult<Bird>> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter)
    {
        var result = new QueryResult<Bird>();

        var query = _dbContext.Birds
            .Include(u => u.BirdConservationStatus) // THIS IS BAD!
            .AsNoTracking()
            .AsQueryable();

        if (speciesFilter == BirderStatus.Common)
        {
            query = query.Where(bs => bs.BirderStatus == BirderStatus.Common);
        }

        query = query.OrderBy(s => s.BirderStatus)
                     .ThenBy(n => n.EnglishName);

        result.TotalItems = await query.CountAsync();

        query = query.ApplyPaging(pageIndex, pageSize);

        result.Items = await query.ToListAsync();

        return result;
    }
}