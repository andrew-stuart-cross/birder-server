﻿using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemClockService _systemClock;
        private readonly IUserRepository _userRepository;
        private readonly IBirdRepository _birdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IObservationRepository _observationRepository;

        public ObservationController(IMapper mapper
                                   , IMemoryCache memoryCache
                                   , ISystemClockService systemClock
                                   , IUnitOfWork unitOfWork
                                   , IUserRepository userRepository
                                   , IBirdRepository birdRepository
                                   , ILogger<ObservationController> logger
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationRepository observationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = memoryCache;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _systemClock = systemClock;
            _userRepository = userRepository;
            _birdRepository = birdRepository;
            _observationRepository = observationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetObservationsAsync()
        {
            ObservationsFeedFilter filter = ObservationsFeedFilter.User;
            try
            {
                var username = User.Identity.Name;

                if (filter == ObservationsFeedFilter.User)
                {
                    var userObservations = await _observationRepository.GetObservationsAsync(o => o.ApplicationUser.UserName == username);
                    if (userObservations.Count() > 0)
                        return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(userObservations));
                }

                if (filter == ObservationsFeedFilter.UserAndNetwork)
                {
                    var loggedinUser = await _userRepository.GetUserAndNetworkAsync(username);

                    var followingUsernamesList = NetworkHelpers.GetFollowingUserNames(loggedinUser.Following);

                    followingUsernamesList.Add(username);

                    var networkObservations = await _observationRepository.GetObservationsAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName));
                    if (networkObservations.Count() > 0)
                        return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(networkObservations));
                }

                //string message = "";
                //if (filter != ObservationsFeedFilter.Public)
                    //message = "There are no observations in your network.  Showing the latest public observations";

                var publicObservations = await _observationRepository.GetObservationsAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public);
                
                if (publicObservations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Observations list is null");
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(publicObservations));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
                return BadRequest("An error occurred getting the observations feed.");
            }
        }

        [HttpGet, Route("GetObservation")]
        public async Task<IActionResult> GetObservation(int id)
        {
            try
            {
                var observation = await _observationRepository.GetObservationAsync(id, true);

                if (observation == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "Observation with id: {ID} was not found.", id);
                    return NotFound();
                }

                return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Observation with id: {ID} was not found.", id);
                return BadRequest("Observation was not found.");
            }
        }

        [HttpGet, Route("GetBirdObservations")]
        public async Task<IActionResult> GetBirdObservationsAsync(int birdId)
        {
            try
            {
                var observations = await _observationRepository.GetObservationsAsync(cs => cs.BirdId == birdId);

                if (observations == null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "GetBirdObservations({ID}) NOT FOUND", birdId);
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<Observation>, IEnumerable<ObservationViewModel>>(observations));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the bird observations list");
                return BadRequest();
            }
        }

        [HttpPost, Route("CreateObservation")]
        public async Task<IActionResult> CreateObservation(ObservationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Identity.Name;

                    var observation = _mapper.Map<ObservationViewModel, Observation>(model);
                    observation.ApplicationUser = await _userManager.FindByNameAsync(username);
                    observation.Bird = await _birdRepository.GetBirdAsync(model.BirdId);
                    observation.CreationDate = _systemClock.GetNow;
                    observation.LastUpdateDate = observation.CreationDate;

                    TryValidateModel(observation);
                    if (!ModelState.IsValid)
                    {
                        // logging
                        return BadRequest(ModelState);
                    }

                    _observationRepository.Add(observation);
                    await _unitOfWork.CompleteAsync();

                    _cache.Remove(CacheEntries.ObservationsList);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
                return BadRequest("An error occurred.  Could not add the observation.");
            }
        }

        [HttpPut, Route("UpdateObservation")]
        public async Task<IActionResult> PutObservation(int id, ObservationViewModel model)
        {
            try
            {
                if (id != model.ObservationId)
                {
                    return BadRequest("An error occurred.  Could not edit the observation.");
                }

                if (ModelState.IsValid)
                {
                    var observation = await _observationRepository.GetObservationAsync(id, true);
                    if (observation == null)
                    {
                        return NotFound();
                    }

                    var username = User.Identity.Name;

                    if (username != observation.ApplicationUser.UserName)
                    {
                        return BadRequest("An error occurred.  You can only edit your own observations.");
                    }

                    _mapper.Map<ObservationViewModel, Observation>(model, observation);

                    observation.Bird = await _birdRepository.GetBirdAsync(model.BirdId);

                    observation.LastUpdateDate = _systemClock.GetNow;

                    TryValidateModel(observation);
                    if (!ModelState.IsValid)
                    {
                        // logging
                        return BadRequest(ModelState);
                    }

                    await _unitOfWork.CompleteAsync();

                    _cache.Remove(CacheEntries.ObservationsList);

                    return Ok(_mapper.Map<Observation, ObservationViewModel>(observation));

                    //try
                    //{
                    //    await _observationRepository.UpdateObservation(observation);
                    //}
                    //catch (DbUpdateConcurrencyException)
                    //{
                    //    if (await _observationRepository.ObservationExists(id)) // !ObservationExists(id))
                    //    {
                    //        return NotFound();
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}

                }
                return BadRequest("An error occurred.  Could not edit the observation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemNotFound, ex, "An error occurred updating (PUT) observation with id: {ID}", id);
                return BadRequest("An error occurred.  Could not edit the observation.");
            }
        }

        [HttpDelete, Route("DeleteObservation")]
        public async Task<ActionResult<ObservationViewModel>> DeleteObservation(int id)
        {
            var observation = await _observationRepository.GetAsync(id);
            if (observation == null)
            {
                return NotFound();
            }

            _observationRepository.Remove(observation);
            await _unitOfWork.CompleteAsync();

            _cache.Remove(CacheEntries.ObservationsList);

            return Ok(id);
        }
    }
}
