using Microsoft.AspNetCore.Identity;
using System;

namespace Common.Security.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? OtpCode { get; set; }
        public DateTime? OtpExpireDateTime { get; set; }
        public DateTime? OtpCreationDateTime { get; set; }
    }
}