﻿using Agents.API.Entities.Documents;
using Microsoft.AspNetCore.Authorization;

namespace Agents.API.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthAdminAttribute : AuthorizeAttribute
    {
        public AuthAdminAttribute()
        {
            Roles = UserDocument.ADMIN;
        }
    }
}
