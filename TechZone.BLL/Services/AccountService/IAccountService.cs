using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.AccountDTOs;
using TechZone.BLL.DTOs.AccountDTOs.RoleDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.AccountService
{
    public interface IAccountService
    {
        Task<Result<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<Result<string>> LoginAsync(LoginDTO loginDTO);
        Task<Result<string>> CreateRole(RoleAddDTO roleAddDTO);
        Task<Result<string>> AssignRoleToUser(AssignRoleDTO assignRoleDTO);
        Task<List<RoleReadDTO>> GetAllRoles();
        Task<Result<List<ApplicationUserDTO>>> GetAllUsers();
        Task<Result<ApplicationUser>> GetCurrentUser(ClaimsPrincipal user);
        public Task DeleteUser(string userId);
    }
}
