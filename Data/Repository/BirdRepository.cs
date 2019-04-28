﻿using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class BirdRepository : IBirdRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BirdRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Bird> GetBird(int id)
        {
            return (from b in _dbContext.Birds
                    .Include(cs => cs.BirdConservationStatus)
                    where (b.BirdId == id)
                    select b).FirstOrDefaultAsync();
            //return await _dbContext.Birds.SingleOrDefaultAsync(m => m.BirdId == id);
        }

        public async Task<IEnumerable<Observation>> GetBirdObservationsAsync(int birdId)
        {
            return await _dbContext.Observations
                .Include(cs => cs.Bird)
                .Where(cs => cs.BirdId == 1)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bird>> GetBirdSummaryList(BirderStatus birderStatusFilter)
        {
            if (birderStatusFilter == BirderStatus.Common)
            {
                return await _dbContext.Birds
                                .Include(cs => cs.BirdConservationStatus)
                                .Where(f => f.BirderStatus == birderStatusFilter)
                                .OrderBy(a => a.EnglishName)
                                .ToListAsync();
            }
            else
            {
                return await _dbContext.Birds
                                .Include(cs => cs.BirdConservationStatus)
                                .OrderBy(ob => ob.BirderStatus)
                                .ThenBy(a => a.EnglishName)
                                .ToListAsync();
            }
        }
    }
}
