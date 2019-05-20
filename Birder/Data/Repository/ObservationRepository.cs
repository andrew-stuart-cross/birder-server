﻿using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class ObservationRepository : Repository<Observation>, IObservationRepository
    {
        public ObservationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync(Expression<Func<Observation, bool>> predicate)
        {
            // include tags?
            return await _dbContext.Observations
                .Include(y => y.Bird)
                    .ThenInclude(u => u.BirdConservationStatus)
                .Include(au => au.ApplicationUser)
                .Where(predicate)
                .OrderByDescending(d => d.ObservationDateTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Observation> GetObservationAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _dbContext.Observations
                            .Include(au => au.ApplicationUser)
                                .SingleOrDefaultAsync(m => m.ObservationId == id);
            }

            return await _dbContext.Observations
                .Include(au => au.ApplicationUser)
                .Include(b => b.Bird)
                .Include(ot => ot.ObservationTags)
                    .ThenInclude(t => t.Tag)
                        .SingleOrDefaultAsync(m => m.ObservationId == id);
        }

        //public async Task<bool> ObservationExists(int id)
        //{
        //    return await _dbContext.Observations.AnyAsync(e => e.ObservationId == id);
        //}
    }
}