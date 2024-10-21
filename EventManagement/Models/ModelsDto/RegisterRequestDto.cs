﻿using EventManagement.Common;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Models.ModelsDto
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        //public string Role { get; set; } = SD.Role_Customer;
    }
}
