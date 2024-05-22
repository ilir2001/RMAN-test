using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMAN_test.Server.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RMAN_test.Server.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

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
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (appUser != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(appUser, loginRequest.Password, true, false);
                    if (result.Succeeded)
                    {
                        return Ok(new { Message = "Login successful." });
                    }
                }
                ModelState.AddModelError(string.Empty, "Login Failed: Invalid Email or Password");
            }

            return BadRequest(ModelState);
        }

        [Route("api/check")]
        [HttpGet]
        public IActionResult CheckLoginStatus()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                string userName = User.FindFirst(ClaimTypes.Name)?.Value;

                return Ok(new { loggedIn = true, userName });
            }

            return Ok(new { loggedIn = false });
        }

        [Route("api/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful." });
        }

        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

    }
}