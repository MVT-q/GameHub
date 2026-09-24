using GameHubAPI.Data;
using GameHubAPI.DTOs.Auth;
using GameHubAPI.Models;
using GameHubAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameHubAPI.Sevices
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly JwtOptions _jwtOptions;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task RegisterAsync(RegisterDto request)
        {
            var user = new User
            {
                Username = request.Username
            };

            var passwordHash = _passwordHasher.HashPassword(user, request.Password);

            user.SetPasswordHash(passwordHash);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if(result == PasswordVerificationResult.Failed)
                return null;

            return new LoginResponseDto
            {
                Token = GenerateJwtToken(user)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var credentials = new SigningCredentials(
                key, 
                algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
