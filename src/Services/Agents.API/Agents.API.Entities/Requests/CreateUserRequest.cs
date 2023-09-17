using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public struct CreateUserRequest
    {

        /// <summary>
        /// User login.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login must be set.")]
        public string Login { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must be set.")]
        public string Name { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password must be set.")]
        public string Password { get; set; }
    }
}
