﻿namespace WebApiApplication.DTO
{
    public class RegisterDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}
