﻿using Birder.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IBirdRepository
    {
        IQueryable<Bird> GetBirdSummaryList();
        IQueryable<Observation> GetBirdObservationsAsync(int birdId);
        Task<Bird> GetBird(int id);
    }
}
