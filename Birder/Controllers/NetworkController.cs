﻿using AutoMapper;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class NetworkController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INetworkRepository _networkRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserNetworkHelpers _networkHelpers;

    public NetworkController(IMapper mapper
                           , IUnitOfWork unitOfWork
                           , ILogger<NetworkController> logger
                           , INetworkRepository networkRepository
                           , UserManager<ApplicationUser> userManager
                           , IUserNetworkHelpers networkHelpers)
    {
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _networkRepository = networkRepository;
        _networkHelpers = networkHelpers;
    }

    [HttpGet]
    public async Task<IActionResult> GetNetworkSummaryAsync()
    {
        try
        {
            var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "requesting user not found");
                return StatusCode(500, "requesting user not found");
            }

            var model = new NetworkSummaryDto
            {
                FollowersCount = requestingUser.Followers.Count,
                FollowingCount = requestingUser.Following.Count
            };

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "an unexpected error occurred");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("Followers")]
    public async Task<IActionResult> GetFollowersAsync(string requestedUsername)
    {
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.GetListNotFound, $"{nameof(requestedUsername)} method argument is null or empty");
            return BadRequest();
        }

        try
        {
            var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

            if (requestedUser is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "UserManager returned null");
                return StatusCode(500);
            }

            var model = _mapper.Map<ICollection<Network>, IEnumerable<FollowerViewModel>>(requestedUser.Followers);

            var requesterUsername = User.Identity.Name;

            if (requesterUsername.Equals(requestedUsername))
            {
                // Own profile requested...
                _networkHelpers.SetupFollowersCollection(requestedUser, model);
                return Ok(model);
            }
            else
            {
                // Other user's profile requested...
                var requestingUser = await _userManager.GetUserWithNetworkAsync(requesterUsername);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, $"Username '{requesterUsername}' not found at GetUserProfileAsync action");
                    return StatusCode(500);
                }

                _networkHelpers.SetupFollowersCollection(requestingUser, model);
                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpGet, Route("Following")]
    public async Task<IActionResult> GetFollowingAsync(string requestedUsername)
    {
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.GetListNotFound, $"{nameof(requestedUsername)} method argument is null or empty");
            return BadRequest();
        }

        try
        {
            var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

            if (requestedUser is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "UserManager returned null");
                return StatusCode(500);
            }

            var model = _mapper.Map<ICollection<Network>, IEnumerable<FollowingViewModel>>(requestedUser.Following);

            var requesterUsername = User.Identity.Name;

            if (requesterUsername.Equals(requestedUsername))
            {
                // Own profile requested...
                _networkHelpers.SetupFollowingCollection(requestedUser, model);
                return Ok(model);
            }
            else
            {
                // Other user's profile requested...
                var requestingUser = await _userManager.GetUserWithNetworkAsync(requesterUsername);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, $"Username '{requesterUsername}' not found at GetUserProfileAsync action");
                    return StatusCode(500, "requesting user not found");
                }

                _networkHelpers.SetupFollowingCollection(requestingUser, model);
                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpGet, Route("Suggestions")]
    public async Task<IActionResult> GetNetworkSuggestionsAsync()
    {
        try
        {
            var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "UserManager returned null");
                return StatusCode(500);
            }

            var followersNotBeingFollowed = _networkHelpers.GetFollowersNotBeingFollowedUserNames(requestingUser);

            if (followersNotBeingFollowed.Any())
            {
                var users = await _userManager.GetUsersAsync(user => followersNotBeingFollowed.Contains(user.UserName));
                var model = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users);
                return Ok(model);
            }
            else
            {
                var followingUsernamesList = _networkHelpers.GetFollowingUserNames(requestingUser.Following);
                var users = await _userManager.GetUsersAsync(user => !followingUsernamesList.Contains(user.UserName) && user.UserName != requestingUser.UserName);
                var model = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users);
                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpGet, Route("Search")]
    public async Task<IActionResult> GetSearchNetworkAsync(string searchCriterion)
    {
        try
        {
            if (string.IsNullOrEmpty(searchCriterion))
            {
                _logger.LogError(LoggingEvents.GetListNotFound, "The search criterion is null or empty");
                return BadRequest("No search criterion");
            }

            var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "The user was not found");
                return StatusCode(500, "requesting user not found");
            }

            var followingUsernamesList = _networkHelpers.GetFollowingUserNames(requestingUser.Following);
            followingUsernamesList.Add(requestingUser.UserName);

            //var users = await _SearchBirdersToFollowAsync(searchCriterion, followingUsernamesList);
            var users = await _userManager.GetUsersAsync(user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(user.UserName));
            return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "an unexpected error occurred");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpPost, Route("Follow")]
    public async Task<IActionResult> PostFollowUserAsync(NetworkUserViewModel userToFollowDetails)
    {
        try
        {
            var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, "Requesting user not found");
                // 500 error and change message
                return NotFound("Requesting user not found");
            }

            var userToFollow = await _userManager.GetUserWithNetworkAsync(userToFollowDetails.UserName);

            if (userToFollow is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, "User to follow not found");
                // 500 error and change message
                return NotFound("User to follow not found");
            }

            if (requestingUser == userToFollow)
            {
                // logger
                return BadRequest("Trying to follow yourself");
            }

            _networkRepository.Follow(requestingUser, userToFollow);

            await _unitOfWork.CompleteAsync();

            var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);

            viewModel.IsFollowing = _networkHelpers.UpdateIsFollowing(viewModel.UserName, requestingUser.Following);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "an unexpected error occurred");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpPost, Route("Unfollow")]
    public async Task<IActionResult> PostUnfollowUserAsync(NetworkUserViewModel userToUnfollowDetails) //, int currentPage)
    {
        try
        {
            var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUser is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, "Requesting user not found");
                // 500 error and change message
                return NotFound("Requesting user not found");
            }

            var userToUnfollow = await _userManager.GetUserWithNetworkAsync(userToUnfollowDetails.UserName);

            if (userToUnfollow is null)
            {
                _logger.LogError(LoggingEvents.UpdateItem, "User to Unfollow not found");
                // 500 error and change message
                return NotFound("User to Unfollow not found");
            }

            if (requestingUser == userToUnfollow)
            {
                // logger
                return BadRequest("Trying to unfollow yourself");
            }

            _networkRepository.Unfollow(requestingUser, userToUnfollow);

            await _unitOfWork.CompleteAsync();

            var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);
            viewModel.IsFollowing = _networkHelpers.UpdateIsFollowing(viewModel.UserName, requestingUser.Following);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "an unexpected error occurred");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}