﻿namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuthenticationTokenService _authenticationTokenService;
    private readonly ILogger _logger;

    public AuthenticationController(UserManager<ApplicationUser> userManager
                                    , SignInManager<ApplicationUser> signInManager
                                    , ILogger<AuthenticationController> logger
                                    , IAuthenticationTokenService authenticationTokenService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationTokenService = authenticationTokenService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)  // [FromBody] removed
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.UserName);

            if (user is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "Login failed: User not found");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
            }

            if (user.EmailConfirmed == false)
            {
                _logger.LogInformation("You cannot login until you confirm your email.");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.EmailConfirmationRequired });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.LockedOut });
            }

            if (result.Succeeded)
            {
                var token = _authenticationTokenService.CreateToken(user);
                
                var viewModel = new AuthenticationResultDto()
                {
                    FailureReason = AuthenticationFailureReason.None,
                    AuthenticationToken = token
                };

                return Ok(viewModel);
            }

            _logger.LogWarning(LoggingEvents.GenerateItems, "other authentication failure");
            return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, ex.Message);
            return StatusCode(500, new AuthenticationResultDto() { FailureReason = AuthenticationFailureReason.Other });
        }
    }
}