﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = new DateTimeOffset(DateTime.Now);
        public DateTimeOffset UpdatedAt { get; set; } = new DateTimeOffset(DateTime.Now);

        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}