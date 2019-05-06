﻿using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ObservationAnalysisController : ControllerBase
    {
        private readonly ISystemClock _systemClock;
        private readonly IObservationsAnalysisRepository _observationsAnalysisRepository;

        public ObservationAnalysisController(IObservationsAnalysisRepository observationsAnalysisRepository
                                            , ISystemClock systemClock)
        {
            _systemClock = systemClock;
            _observationsAnalysisRepository = observationsAnalysisRepository;

        }

        [HttpGet, Route("GetObservationAnalysis")]
        public async Task<IActionResult> GetObservationAnalysis()
        {
            var username = User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var viewModel = await _observationsAnalysisRepository.GetObservationsAnalysis(username);

            return Ok(viewModel);
        }

        [HttpGet, Route("GetTopObservationAnalysis")]
        public IActionResult GetTopObservationAnalysis()
        {
            var username = User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var viewModel = new TopObservationsAnalysisViewModel()
            {
                TopObservations = _observationsAnalysisRepository.GetTopObservations(username),
                TopMonthlyObservations = _observationsAnalysisRepository.GetTopObservations(username, _systemClock.GetToday.AddDays(-30))
            };

            return Ok(viewModel);
        }

        [HttpGet, Route("GetLifeList")]
        public IActionResult GetLifeList()
        {
            var username = User.Identity.Name;

            if (username == null)
            {
                return Unauthorized();
            }

            var viewModel = new LifeListViewModel()
            {
                LifeList = _observationsAnalysisRepository.GetLifeList(username)
            };

            return Ok(viewModel);
        }
    }
}
