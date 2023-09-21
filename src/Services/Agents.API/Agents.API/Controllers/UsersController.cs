using Agents.API.Data.Store;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Sky.Controllers
{
    [Route("api/agents/[controller]")]
    [ApiController]
    [AuthAdmin]
    public class UsersController : ControllerBase
    {
        private readonly UsersStore _usersStore;

        public UsersController(UsersStore usersStore = null)
        {
            _usersStore = usersStore;
        }


        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            if (_usersStore == null)
                return Ok(Enumerable.Empty<UserDocument>());

            var users = await _usersStore.All();
            return Ok(users);
        }


        /// <summary>
        /// Create user by creation request
        /// </summary>
        /// <param name="request">creation request</param>
        /// <returns>Ok if success, BadRequest if login is already existing.</returns>
        [HttpPost("create")]
        public async Task<ActionResult> CreateUser(CreateUserRequest request)
        {
            if (_usersStore == null)
                return Ok();

            if (await _usersStore.IsUserExist(request.Login))
                return BadRequest($"User with login {request.Login} is already existing.");

            UserDocument doc = new UserDocument
            {
                Login = request.Login,
                Name = request.Name,
                Role = UserDocument.EDITOR,
                RegistrationDate = DateTime.Today
            };
            doc.SetPassword(request.Password);
            await _usersStore.Insert(doc);
            return Ok();
        }


        /// <summary>
        /// Update user by request.
        /// </summary>
        /// <param name="request">updating request.</param>
        /// <returns></returns>
        [HttpPatch("update")]
        public async Task<ActionResult> UpdateUser(UpdateUserRequest request)
        {
            if (_usersStore == null)
                return Ok();

            var user = await _usersStore.UpdateUser(request);
            if (HttpContext.User.Identity?.Name == user.Login && user.IsBan)
                await HttpContext.SignOutAsync();
            return Ok();
        }


        public class SetPasswordRequest
        {
            /// <summary>
            /// User database id.
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "UserId must be set.")]
            public string UserId { get; set; }

            /// <summary>
            /// User password.
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Password must be set.")]
            public string Password { get; set; }
        }


        /// <summary>
        /// Set password for a user.
        /// </summary>
        /// <param name="request">set password request.</param>
        /// <returns></returns>
        [HttpPost("setpassword")]
        public async Task<ActionResult> SetPassword([FromBody] SetPasswordRequest request)
        {
            if (_usersStore == null)
                return Ok();

            await _usersStore.Update()
                .Where(u => u.Id == request.UserId)
                .Set(u => u.Password, UserDocument.HashPassword(request.Password))
                .Execute();
            return Ok();
        }


        /// <summary>
        /// Delete user by database id.
        /// </summary>
        /// <param name="userId">Database id.</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            if (_usersStore == null)
                return Ok();

            await _usersStore.Delete(x => x.Id == userId);
            return Ok();
        }
    }
}
