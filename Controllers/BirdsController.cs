﻿using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BirdsController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IBirdRepository _birdRepository;

        public BirdsController(IMapper mapper
                             , IMemoryCache memoryCache
                             , ILogger<BirdsController> logger
                             , IBirdRepository birdRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _birdRepository = birdRepository;
        }

        // No need for server side pagination.  Birds list is the same as the request for the drop down lists!

        [HttpGet]
        public IActionResult GetBirds(BirderStatus filter) // int pageIndex, int pageSize)
        {
            try
            {
                if (_cache.TryGetValue("AllBirdsList", out IEnumerable<BirdSummaryViewModel> birdsCache))
                {
                    if (filter == BirderStatus.Common)
                    {
                        var commonBirdsCache = (from birds in birdsCache
                                           where birds.BirderStatus == "Common"
                                           select birds);
                        return Ok(commonBirdsCache);
                    }
                    else
                    {
                        return Ok(birdsCache);
                    }
                }
                else
                {
                    var birds = _birdRepository.GetBirdSummaryList();

                    if (birds == null)
                    {
                        _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                        return BadRequest();
                    }

                    var viewModel = _mapper.Map<IEnumerable<Bird>, IEnumerable<BirdSummaryViewModel>>(birds);

                    _cache.Set("AllBirdsList", viewModel, TimeSpan.FromMinutes(10));

                    if (filter == BirderStatus.Common)
                    {
                        var filteredViewModel = (from items in birdsCache
                                           where items.BirderStatus == "Common"
                                           select items);
                        return Ok(filteredViewModel);
                    }

                    return Ok(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
                return BadRequest();
            }
        }

        [HttpGet, Route("GetBird")]
        public async Task<IActionResult> GetBird(int id)
        {
            try
            {
                var bird = await _birdRepository.GetBird(id);

                if (bird == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetBird({ID}) NOT FOUND", id);
                    return NotFound();
                }

                var viewmodel = _mapper.Map<Bird, BirdDetailViewModel>(bird);

                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting bird with {ID}", id);
                return BadRequest();
            }
        }

        [HttpGet, Route("GetObservations")]
        public IActionResult GetBirdObservations(int birdId)
        {
            try
            {
                var observations = _birdRepository.GetBirdObservationsAsync(birdId);

                if (observations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "GetBirdObservations({ID}) NOT FOUND", birdId);
                    return NotFound();
                }

                var viewModel = _mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations);

                return Ok(viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the bird observations list");
                return BadRequest();
            }
        }
    }
}
