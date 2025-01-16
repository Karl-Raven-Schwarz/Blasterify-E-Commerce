﻿using System;

namespace Blasterify.Models.Requests
{
    public abstract class SignUpRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public Guid CountryId { get; set; }
    }
}