using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class AccessCode
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int Code { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
    }
}
