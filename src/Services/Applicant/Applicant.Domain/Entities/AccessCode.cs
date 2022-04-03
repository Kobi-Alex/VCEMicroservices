using System;


namespace Applicant.Domain.Entities
{
    public class AccessCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
    }

}
