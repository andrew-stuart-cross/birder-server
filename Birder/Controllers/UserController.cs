﻿using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INetworkRepository _networkRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IMapper mapper
                            , IUnitOfWork unitOfWork
                            , ILogger<UserController> logger
                            , INetworkRepository networkRepository
                            , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _networkRepository = networkRepository;
        }

        [HttpGet, Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync(string requestedUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(requestedUsername))
                {
                    //Bad Request
                    return BadRequest();
                }

                var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

                if (requestedUser == null)
                {
                    return NotFound("Requested user not found");
                }

                var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);

                var requesterUsername = User.Identity.Name;

                if (requesterUsername.Equals(requestedUsername))
                {
                    // Own profile requested
                    requestedUserProfileViewModel.IsOwnProfile = true;

                    UserProfileHelper.UpdateFollowingCollection(requestedUserProfileViewModel, requestedUser); //, loggedinUsername);

                    UserProfileHelper.UpdateFollowersCollection(requestedUserProfileViewModel, requestedUser);

                    return Ok(requestedUserProfileViewModel);
                }

                var requestingUser = await _userManager.GetUserWithNetworkAsync(requesterUsername);

                if (requestingUser == null)
                {
                    return NotFound("Requesting user not found");
                }

                UserProfileHelper.UpdateIsFollowingProperty(requestedUserProfileViewModel, requestedUser, requestingUser);

                UserProfileHelper.UpdateFollowingCollection(requestedUserProfileViewModel, requestingUser); //, loggedinUsername);

                UserProfileHelper.UpdateFollowersCollection(requestedUserProfileViewModel, requestingUser);

                return Ok(requestedUserProfileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Error at GetUserProfileAsync");
                return BadRequest("There was an error getting the user");
            }
        }
    }
}
