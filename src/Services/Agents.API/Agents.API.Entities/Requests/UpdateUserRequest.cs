using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public struct UpdateUserRequest
    {

        /// <summary>
        /// User database id.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserId must be set.")]
        public string UserId { get; set; }

        /// <summary>
        /// User update name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must be set.")]
        public string Name { get; set; }

        /// <summary>
        /// User update role.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Role must be set.")]
        public string Role { get; set; }

        /// <summary>
        /// Ban mark.
        /// </summary>
        public bool IsBan { get; set; }

    }
}
