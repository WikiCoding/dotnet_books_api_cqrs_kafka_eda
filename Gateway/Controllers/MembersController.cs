using Gateway.Dtos;
using Gateway.Persistence;
using Gateway.Services;
using Gateway.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly UsersDbContext _dbContext;
        private readonly AuthService _authService;

        public MembersController(UsersDbContext dbContext, AuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest request)
        {
            if (request.username.Trim().Length == 0 || request.password.Trim().Length == 0) return BadRequest("Invalid parameters");
            var result = await _authService.RegisterUser(request.username, request.password);

            if (result.Value is null) return HandleErrors(result);

            return Ok(new UserResponse(result.Value!.id, result.Value.username, result.Value.password));
        }

        private ObjectResult HandleErrors<T>(ResultResponse<T> result)
        {
            if (result.ErrorType is ErrorType.DuplicatedEntry) return Conflict(result.ErrorMessage);
            if (result.ErrorType is ErrorType.NotFound) return NotFound(result.ErrorMessage);
            if (result.ErrorType is ErrorType.Validation) return BadRequest(result.ErrorMessage);
            return Problem(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserRequest request)
        {
            if (request.username.Trim().Length == 0 || request.password.Trim().Length == 0) return BadRequest("Invalid parameters");

            ResultResponse<string> result = await _authService.LoginUser(request.username, request.password);

            if (result.Value is null) return HandleErrors(result);

            return Ok(new LoginResponse(request.username, result.Value));
        }
    }
}
