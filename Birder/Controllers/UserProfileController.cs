﻿using AutoMapper;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UserProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IObservationsAnalysisService _observationsAnalysisService;
    private readonly IUserNetworkHelpers _networkHelpers;

    public UserProfileController(IMapper mapper
                        , ILogger<UserProfileController> logger
                        , UserManager<ApplicationUser> userManager
                        , IObservationsAnalysisService observationsAnalysisService
                        , IUserNetworkHelpers networkHelpers)
    {
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _observationsAnalysisService = observationsAnalysisService;
                _networkHelpers = networkHelpers;
    }

    // todo: create QueryObject
    [HttpGet]
    public async Task<IActionResult> GetUserProfileAsync(string requestedUsername)
    {
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.GetItem, "requestedUsername argument is null or empty at GetUserProfileAsync action");
            return BadRequest($"{nameof(requestedUsername)} argument is null or empty");
        }

        try
        {
            var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

            if (requestedUser is null)
            {
                _logger.LogError(LoggingEvents.GetItem, $"Username '{requestedUsername}' not found at GetUserProfileAsync action");
                return StatusCode(500, "userManager returned null");
            }

            var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);


            if (requestedUsername.Equals(User.Identity.Name))
            {
                // Own profile requested...
                requestedUserProfileViewModel.User.IsOwnProfile = true;
            }
            else
            {
                // Other user's profile requested...
                requestedUserProfileViewModel.User.IsFollowing = _networkHelpers.UpdateIsFollowingProperty(User.Identity.Name, requestedUser.Followers);
                requestedUserProfileViewModel.ObservationCount = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);
            }

            return Ok(requestedUserProfileViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "Error at GetUserProfileAsync");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}