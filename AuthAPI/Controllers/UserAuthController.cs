using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string? _jwtKey;
        private readonly string? _issuer;
        private readonly string? _jwtAudence;
        private readonly int _jwtExpiry;

        public UserAuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _jwtAudence = configuration["Jwt:Audience"];
            _jwtExpiry = int.Parse(configuration["Jwt:ExpiresInMinutes"]);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if(registerModel == null 
                || string.IsNullOrEmpty(registerModel.Email) 
                || string.IsNullOrEmpty(registerModel.Password)
                || string.IsNullOrEmpty(registerModel.Name))
            {
                return BadRequest("Invalid Registeration details");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }

            var newUser = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                Name = registerModel.Name
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User created successfully");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if(user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if(!result.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }

            var token = GenerateJWTToken(user);

            return Ok(new { success = true, token }); 
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User Logged out Successfully");
        }

        private string GenerateJWTToken(ApplicationUser user)
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id),
                new Claim(JwtRegisteredClaimNames.Email , user.Email ),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim("Name", user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddMinutes(_jwtExpiry),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
