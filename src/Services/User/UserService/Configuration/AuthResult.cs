using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dto.User;
using UserService.Models;

namespace UserService.Configuration
{
    public class AuthResult
    {
        public UserReadDto User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Error { get; set; }
    }
}
