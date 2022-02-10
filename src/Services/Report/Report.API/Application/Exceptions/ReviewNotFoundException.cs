using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.API.Application.Exceptions
{
    public sealed class ReviewNotFoundException : NotFoundException
    {
        public ReviewNotFoundException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}