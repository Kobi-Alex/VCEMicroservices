using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Application.Exceptions
{
    public class BadRequestMessage : BadRequestException
    {
        public BadRequestMessage(string message)
            : base(message)
        {
        }
    }
}
