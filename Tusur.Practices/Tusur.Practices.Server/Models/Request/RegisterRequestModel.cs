﻿namespace Tusur.Practices.Server.Models.Request
{
    public class RegisterRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
    }
}
