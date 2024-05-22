// Controllers/UserController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMAN_test.Server.Models;
using RMAN_test.Server.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RMAN_test.Server.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("api/create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser appUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            IdentityResult result = await _userRepository.CreateUserAsync(appUser, user.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User Created Successfully" });
            }
            else
            {
                return StatusCode(500, new { Errors = result.Errors.Select(error => error.Description) });
            }
        }

        [Route("api/login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest(new { Message = "You are already logged in." });
            }
            if (ModelState.IsValid)
            {
                var appUser = await _userRepository.FindByEmailAsync(loginRequest.Email);
                if (appUser != null)
                {
                    await _userRepository.SignOutAsync();
                    var result = await _userRepository.PasswordSignInAsync(appUser, loginRequest.Password, true, false);
                    if (result.Succeeded)
                    {
                        return Ok(new { Message = "Login successful." });
                    }
                }
                ModelState.AddModelError(string.Empty, "Login Failed: Invalid Email or Password");
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [Route("api/check")]
        [HttpGet]
        public IActionResult CheckLoginStatus()
        {
            if (_userRepository.IsUserAuthenticated(User))
            {
                string userName = _userRepository.GetUserName(User);
                return Ok(new { loggedIn = true, userName });
            }

            return Ok(new { loggedIn = false });
        }

        [Authorize]
        [Route("api/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userRepository.SignOutAsync();
            return Ok(new { Message = "Logout successful." });
        }

        [Authorize]
        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }
    }
}
