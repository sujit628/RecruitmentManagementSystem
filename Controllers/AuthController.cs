using Microsoft.AspNetCore.Mvc;
using RecruitmentManagementSystem.DTOs;
using RecruitmentManagementSystem.Services;


namespace RecruitmentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(null, new { user.Id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.ValidateUserAsync(dto.Email, dto.Password);
            if (user == null) return Unauthorized(new { message = "Invalid credentials" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new { user.Id, user.Name, user.Email, role = user.UserType }
            });
        }
    }

    //public class AuthController : ControllerBase
    //{
    //    private readonly IUserService _userService;
    //    private readonly IJwtService _jwtService;


    //    public AuthController(IUserService userService, IJwtService jwtService)
    //    {
    //        _userService = userService;
    //        _jwtService = jwtService;
    //    }


    //    [HttpPost("signup")]
    //    public async Task<IActionResult> Signup([FromBody] SignupDto dto)
    //    {
    //        try
    //        {
    //            var user = await _userService.CreateUserAsync(dto);
    //            return Ok(new { message = "User created", userId = user.Id });
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new { error = ex.Message });
    //        }
    //    }


    //    [HttpPost("login")]
    //    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    //    {
    //        var user = await _userService.ValidateUserAsync(dto.Email, dto.Password);
    //        if (user == null) return Unauthorized(new { message = "Invalid credentials" });


    //        var token = _jwtService.GenerateToken(user);
    //        return Ok(new AuthResponseDto(token));
    //    }
    //}
}