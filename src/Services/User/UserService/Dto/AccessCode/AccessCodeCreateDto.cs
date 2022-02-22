using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dto.AccessCode
{
    public class AccessCodeCreateDto
    {
        public string Email { get; set; }

        public int Code { get; set; }
    }
}
