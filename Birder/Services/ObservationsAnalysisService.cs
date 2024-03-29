﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Services;

public interface IObservationsAnalysisService
{
    Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate);
}

public class ObservationsAnalysisService : IObservationsAnalysisService
{
    private readonly ApplicationDbContext _dbContext;
    public ObservationsAnalysisService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ObservationAnalysisViewModel> GetObservationsSummaryAsync(Expression<Func<Observation, bool>> predicate)
    {
        if (predicate is null)
            throw new ArgumentException("method argument is null or empty", nameof(predicate));

        var query = _dbContext.Observations
            .Include(au => au.ApplicationUser)
            .AsNoTracking()
            .AsQueryable();

        query = query.Where(predicate);

        var model = new ObservationAnalysisViewModel();
        model.TotalObservationsCount = await query.CountAsync();
        model.UniqueSpeciesCount = await query.Select(b => b.BirdId).Distinct().CountAsync();

        return model;
    }
}