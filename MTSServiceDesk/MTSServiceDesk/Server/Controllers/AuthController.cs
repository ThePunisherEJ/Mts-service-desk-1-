using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTS.ServiceDesk.Server.Models;
using MTS.ServiceDesk.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.ServiceDesk.Server.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return BadRequest("User does not exist");
            if(user.UserStatusId != 1) return BadRequest("Inactive User");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Invalid password");
            await _signInManager.SignInAsync(user, request.RememberMe);
            return Ok();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            var user = new ApplicationUser();
            user.UserName = parameters.UserName;
            user.FirstName = "TBC";
            user.LastName = "TBC";
            user.UserStatusId = 1;
            user.ClientId = 1;
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);
            return await Login(new LoginRequest
            {
                UserName = parameters.UserName,
                Password = parameters.Password
            });
        }



        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }


        [HttpGet("currentuserinfo")]
        public CurrentUser CurrentUserInfo()
        {
            return new CurrentUser
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                //Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
                Claims = User.Claims.Select(x => new string[2] { x.Type, x.Value }).ToList()

            };
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserCreateUpdateRequest newUser)
        {
            try
            {
                var user = new ApplicationUser();
                user.UserName = newUser.Email;
                user.Email = newUser.Email;
                user.ClientId = newUser.ClientId;
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.UserStatusId = newUser.UserStatus;


                user.UserStatusId = 1;
                var result = await _userManager.CreateAsync(user, "Abc654321!");
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }

                switch (newUser.TypeOfUser)
                {
                    case UserType.Administrator:
                        await _userManager.AddToRoleAsync(user, "Admin");
                        await _userManager.AddToRoleAsync(user, "Consultant");
                        break;
                    case UserType.Consultant:
                        await _userManager.AddToRoleAsync(user, "Consultant");
                        break;
                    case UserType.User:
                        await _userManager.AddToRoleAsync(user, "User");
                        break;
                    default:
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser(UserCreateUpdateRequest updUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updUser.UserId.ToString());

                user.UserName = updUser.Email;
                user.Email = updUser.Email;
                user.ClientId = updUser.ClientId;
                user.FirstName = updUser.FirstName;
                user.LastName = updUser.LastName;
                user.UserStatusId = updUser.UserStatus;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }
                await _userManager.RemoveFromRolesAsync(user,new List<string> { "Admin", "Consultant", "User" });
                switch (updUser.TypeOfUser)
                {
                    case UserType.Administrator:
                        await _userManager.AddToRoleAsync(user, "Admin");
                        await _userManager.AddToRoleAsync(user, "Consultant");
                        break;
                    case UserType.Consultant:
                        await _userManager.AddToRoleAsync(user, "Consultant");
                        break;
                    case UserType.User:
                        await _userManager.AddToRoleAsync(user, "User");
                        break;
                    default:
                        break;
                }



                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-admin-role")]
        public async Task<IActionResult> AddAdminRole(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPost("remove-admin-role")]
        public async Task<IActionResult> RemoveAdminRole(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<UserDetails> userList = new List<UserDetails>();
                var appUserList = _userManager.Users.ToList();
                foreach (var item in appUserList)
                {
                    bool isAdmin = await _userManager.IsInRoleAsync(item, "Admin");
                    bool isCons = await _userManager.IsInRoleAsync(item, "Consultant");
                    bool isUser = await _userManager.IsInRoleAsync(item, "User");
                    UserType usrType = UserType.User;
                    if (isAdmin)
                    {
                        usrType = UserType.Administrator;
                    }
                    if (isCons && !isAdmin)
                    {
                        usrType = UserType.Consultant;
                    }
                    userList.Add(new UserDetails
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        UserId = Guid.Parse(item.Id),
                        ClientId = item.ClientId,
                        UserStatus = item.UserStatusId,
                        TypeOfUser = usrType
                    });
                }
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-users-by-clientid/{clientId}")]
        public async Task<IActionResult> GetUsersForClient(string clientId)
        {
            try
            {
                List<UserDetails> userList = new List<UserDetails>();
                int clientIdInt = int.Parse(clientId);
                var appUserList = _userManager.Users.Where(u => u.ClientId == clientIdInt ).ToList();
                foreach (var item in appUserList)
                {
                    bool isAdmin = await _userManager.IsInRoleAsync(item, "Admin");
                    bool isCons = await _userManager.IsInRoleAsync(item, "Consultant");
                    bool isUser = await _userManager.IsInRoleAsync(item, "User");
                    UserType usrType = UserType.User;
                    if (isAdmin)
                    {
                        usrType = UserType.Administrator;
                    }
                    if (isCons && !isAdmin)
                    {
                        usrType = UserType.Consultant;
                    }
                    userList.Add(new UserDetails
                    {
                        Email = item.Email,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        UserId = Guid.Parse(item.Id),
                        ClientId = item.ClientId,
                        UserStatus = item.UserStatusId,
                        TypeOfUser = usrType
                    });
                }
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-user-by-id/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(userId);
                bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
                bool isCons = await _userManager.IsInRoleAsync(appUser, "Consultant");
                bool isUser = await _userManager.IsInRoleAsync(appUser, "User");
                UserType usrType = UserType.User;
                if (isAdmin)
                {
                    usrType = UserType.Administrator;
                }
                if (isCons && !isAdmin)
                {
                    usrType = UserType.Consultant;
                }

                UserDetails user = new UserDetails
                {
                    Email = appUser.Email,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    UserId = Guid.Parse(appUser.Id),
                    ClientId = appUser.ClientId,
                    TypeOfUser = usrType,
                    UserStatus = appUser.UserStatusId
                    
                };
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("disable-user/{Id}")]
        public async Task<IActionResult> DisableUserAsync(string Id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(Id);
                appUser.UserStatusId = 2;
                var result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpPost("enable-user/{Id}")]
        public async Task<IActionResult> EnableUserAsync(string Id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(Id);
                appUser.UserStatusId = 1;
                var result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
