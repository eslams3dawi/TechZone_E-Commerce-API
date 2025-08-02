using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.DTOs.AccountDTOs;
using TechZone.BLL.DTOs.AccountDTOs.RoleDTOs;
using TechZone.BLL.Services.AccountService;
using TechZone.BLL.Wrappers;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result<string>>> Register(RegisterDTO registerDTO)
        {
            var result = await _accountService.RegisterAsync(registerDTO);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Result<string>>> Login(LoginDTO loginDTO)
        {
            var result = await _accountService.LoginAsync(loginDTO);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleAddDTO roleAddDTO)
        {
            var result = await _accountService.CreateRole(roleAddDTO);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDTO)
        {
            var result = await _accountService.AssignRoleToUser(assignRoleDTO);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _accountService.GetAllRoles();

            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountService.GetAllUsers();

            return Ok(users);
        }

        [Authorize(Roles = "User")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetCurrentAccount()
        {
            var user = await _accountService.GetCurrentUser(User);

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            await _accountService.DeleteUser(userId);

            return NoContent();
        }
    }
}
