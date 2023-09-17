using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Agents.API.Entities.Mongo;
using Agents.API.Data.Store;

namespace Agents.API.Controllers
{
    [Route("api/agents/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected string UserLogin => User.Identity.Name;

        protected bool IsAdmin => User.IsInRole(UserDocument.ADMIN);

        protected UsersStore _usersStore;

        public AuthenticationController(UsersStore usersStore = null)
        {
            _usersStore = usersStore;
        }

        protected ActionResult ApiBadRequest(string message)
        {
            return BadRequest(new { Message = message });
        }

        protected Task Authorize(UserDocument user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { IsPersistent = true };

            return HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> Me()
        {
            if (_usersStore == null)
                return Ok();

            if (UserLogin == null)
                return Ok();

            var user = await _usersStore.Get(u => u.Login == UserLogin);
            if (user == null)
                return Forbid();

            return Ok(new
            {
                user.Id,
                user.Login,
                user.Name,
                user.Role
            });
        }

        public class ChangePasswordRequest
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Password must be set.")]
            public string Password { get; set; }
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (_usersStore == null)
                return Ok();

            await _usersStore.Update()
                .Where(u => u.Login == User.Identity.Name)
                .Set(u => u.Password, UserDocument.HashPassword(request.Password))
                .Execute();
            return Ok();
        }


        public class ChangeNameRequest
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Name must be set.")]
            public string Name { get; set; }
        }

        [Authorize]
        [HttpPost("changename")]
        public async Task<ActionResult> ChangeName([FromBody] ChangeNameRequest request)
        {
            if (_usersStore == null)
                return Ok();

            await _usersStore.Update()
                .Where(u => u.Login == User.Identity.Name)
                .Set(u => u.Name, request.Name)
                .Execute();
            return Ok();
        }


        public class LoginRequest
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Login must be set.")]
            public string Login { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Password must be set.")]
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (_usersStore == null)
                return Ok();

            var user = await _usersStore.Get(u => u.Login == request.Login);
            if (user == null || !user.CheckPassword(request.Password))
                return ApiBadRequest("Wrong login or password.");
            if (user.IsBan)
                return ApiBadRequest("The user is banned.");

            await Authorize(user);
            return Ok();
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

    }
}
