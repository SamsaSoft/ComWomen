﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
    }

    public static class ClaimsPrincipalExtensions 
    {
        public static string Language (this ClaimsPrincipal claims) =>
            claims.FindFirstValue("language");
    }
}
