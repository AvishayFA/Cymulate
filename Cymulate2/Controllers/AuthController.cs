using System.Security.Claims;
using Cymulate2.Models.Entities;
using Cymulate2.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cymulate2.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private SignInManager<ApplicationUser> _signInManager { get; }
    private UserManager<ApplicationUser> _userManager { get; }
    private ILogger<AuthController> _logger { get; }
    private IJwtService _jwtService { get; }

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return BadRequest(new AuthResponse
                {
                    Message = "User with this email already exists",
                    Success = false
                });
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                CreatedAt = DateTime.UtcNow,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var token = _jwtService.GenerateToken(user);

                return Ok(new AuthResponse
                {
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    Message = "User registered successfully",
                    Success = true,
                    Token = token,
                    User = new UserInfo
                    {
                        CreatedAt = user.CreatedAt,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ID = user.Id
                    }
                });
            }
            else
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = $"Registration failed: {errors}"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");

            return StatusCode(500, new AuthResponse
            {
                Success = false,
                Message = "Internal server error occurred"
            });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Unauthorized(new AuthResponse
                {
                    Message = "Invalid email or password",
                    Success = false
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var token = _jwtService.GenerateToken(user);

                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = new UserInfo
                    {
                        CreatedAt = user.CreatedAt,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ID = user.Id
                    }
                });
            }
            else
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");

            return StatusCode(500, new AuthResponse
            {
                Success = false,
                Message = "Internal server error occurred"
            });
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserInfo>> GetProfile()
    {
        try
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserInfo
            {
                ID = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");

            return StatusCode(500, "Internal server error occurred");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok(new { message = "Logged out successfully" });
    }
}
