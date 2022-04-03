using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Applicant.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }

}
