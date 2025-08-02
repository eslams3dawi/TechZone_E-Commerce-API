using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.DTOs.AccountDTOs;
using TechZone.BLL.DTOs.AccountDTOs.RoleDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, 
                                            IConfiguration configuration,
                                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<Result<string>> AssignRoleToUser(AssignRoleDTO assignRoleDTO)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDTO.UserId);
            var role = await _roleManager.FindByIdAsync(assignRoleDTO.RoleId);

            List<string> errors = new List<string>();

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if(result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, role.Name),
                        new Claim(ClaimTypes.Name, user.FirstName)
                    };

                    var resultClaims = await _userManager.AddClaimsAsync(user, claims);

                    if (resultClaims.Succeeded)
                        return Result<string>.Success("Role Assigned Successfully");

                    errors = resultClaims.Errors.Select(e => e.Description).ToList();
                    return Result<string>.Failure("Failed to assign role", errors, ActionCode.BadRequest);
                }
                errors = result.Errors.Select(e => e.Description).ToList();
                return Result<string>.Failure("Failed to assign role", errors, ActionCode.BadRequest);
            }
            return Result<string>.Failure("User or role not exits", null, ActionCode.BadRequest);
        }

        public async Task<Result<string>> CreateRole(RoleAddDTO roleAddDTO)
        {
            var role = new IdentityRole()
            {
                Name = roleAddDTO.Name,
                NormalizedName = roleAddDTO.Name.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
                return Result<string>.Success("Role Created Successfully");

            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<string>.Failure("Failed to create role", errors, ActionCode.BadRequest);
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new BadRequestException($"User with Id: {userId} not found");

            var result = await _userManager.DeleteAsync(user);
        }

        public async Task<List<RoleReadDTO>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleReadDTO()
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();

            return roles;
        }

        public async Task<Result<List<ApplicationUserDTO>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = users.Select(u => new ApplicationUserDTO
            {
                UserId = u.Id,
                Email = u.Email
            }).ToList();

            return Result<List<ApplicationUserDTO>>.Success(result);
        }

        public async Task<Result<ApplicationUser>> GetCurrentUser(ClaimsPrincipal user)
        {
            var userId = user.FindFirst("userId").Value;
            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
                return Result<ApplicationUser>.Failure("User not found", null, ActionCode.NotFound);

            return Result<ApplicationUser>.Success(appUser);
        }

        public async Task<Result<string>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return Result<string>.Failure("Username or password is wrong", null, ActionCode.BadRequest);

            var IsValidPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if(!IsValidPassword)
                return Result<string>.Failure("Username or password is wrong", null, ActionCode.BadRequest);

            var claims = new List<Claim>()
            {
                new Claim("userId", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = GenerateToken(claims);

                return Result<string>.SuccessAndSendToken(token, userRoles);
        }

        public async Task<Result<string>> RegisterAsync(RegisterDTO registerDTO)
        {

            if (registerDTO.Password != registerDTO.PasswordConfirmed)
                return Result<string>.Failure("Passwords Do Not Match", null, ActionCode.BadRequest);

            var user = new ApplicationUser()
            {
                UserName = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                BirthDate = registerDTO.BirthDate
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<string>.Failure("Registration Failed", errors, ActionCode.BadRequest);
            }

            await _userManager.AddToRoleAsync(user, "User");
            return Result<string>.Success("Registered Successfully");
        }
        private string GenerateToken(IList<Claim> claims)
        {
            var securityKeyString = _configuration.GetSection("SecretKey").Value;
            var securityKeyByte = Encoding.ASCII.GetBytes(securityKeyString);

            SecurityKey securityKey = new SymmetricSecurityKey(securityKeyByte);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expire = DateTime.UtcNow.AddHours(5);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims,expires: expire, signingCredentials: signingCredentials);

            //The handler is responsible for converting the format of token (string to object) and vice versa
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var Token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return Token;
        }
    }
}
