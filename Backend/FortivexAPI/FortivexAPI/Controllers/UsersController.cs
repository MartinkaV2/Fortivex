using FortivexAPI.Dtos;
using FortivexAPI.Helpers;
using FortivexAPI.Models;
using FortivexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FortivexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;
        // We no longer keep a hard‑coded JWT key or unused fields here.  The
        // JwtHelper takes care of reading the secret key from configuration,
        // so the controller only needs to delegate token generation.
        private readonly IConfiguration _configuration;

        public UsersController(UserService userService, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        // Accept both PUT and POST to maintain backwards compatibility with earlier frontend versions
        [HttpPost("login")]
        [HttpPut("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userService.GetUserAsync(loginRequest.UserName, loginRequest.PasswordHash);
            if (user == null)
                return Unauthorized("Hibás bejelentkezés!");
            // Generate the JWT via the helper which uses configuration to
            // obtain the secret key and constructs the appropriate token.
            var token = _jwtHelper.GenerateToken(user);
            return Ok(new LoginResponseDto { Token = token });
        }

        [HttpGet("me")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetMyData()
        {
            return Ok($"Bejelentkezve mint: {User.Identity?.Name}");
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok("Csak az Admin látja ezt az adatot!");
        }

        [HttpPost("register")]
        [HttpPut("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            if (await _userService.GetUserByUsernameAsync(registerRequest.UserName) is User existingUser)
                return BadRequest("A felhasználónév már foglalt.");
            var newUser = new User
            {
                Username = registerRequest.UserName,
                PasswordHash = registerRequest.PasswordHash,
                Email = registerRequest.Email,
                Role = registerRequest.Role
            };
            await _userService.CreateUserAsync(newUser);
            return Ok("Sikeres regisztráció!");
        }



    }
}
