using Gateway.Dtos;
using Gateway.Persistence;
using Gateway.Services.JwtProvider;
using Gateway.Services.PasswordHashing;
using Gateway.Shared;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services
{
    public class AuthService
    {
        private readonly UsersDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(UsersDbContext dbContext, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<ResultResponse<UserDto>> RegisterUser(string username, string password)
        {
            var hashed = _passwordHasher.Hash(password);

            User user = new() { Username = username, Password = hashed };

            _dbContext.Add(user);

            var rowsAffected = await _dbContext.SaveChangesAsync();

            if (rowsAffected == 0) return new ResultResponse<UserDto>() { ErrorType = ErrorType.DatabaseException, ErrorMessage = "Error Saving User." };

            return new ResultResponse<UserDto>() { Value = new UserDto(user.Id, user.Username, user.Password) };

        }

        public async Task<ResultResponse<string>> LoginUser(string username, string password)
        {
            User? user = await _dbContext.Users.Where(user => user.Username == username).FirstOrDefaultAsync();

            if (user is null) return new ResultResponse<string>() { ErrorType = ErrorType.NotFound, ErrorMessage = "User not Found" };

            bool match = _passwordHasher.Verify(password, user.Password);

            if (!match) return new ResultResponse<string>() { ErrorType = ErrorType.Validation, ErrorMessage = "Wrong credentials" };

            string token = _jwtProvider.GenerateJwt(user);

            return new ResultResponse<string>() { Value = token };
        }
    }
}
