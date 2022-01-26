using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Report.Domain.SeedWork;

namespace Report.Domain.AggregatesModel.ApplicantAggregate
{
    public class Applicant : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Patronymic { get; private set; }
        public DateTime Birthday { get; private set; }
        public Address Address { get; private set; }


        protected Applicant()
        {
        }

        public Applicant(string identity, string name, string surname, string patronymic, DateTime birthday, Address address) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Surname = !string.IsNullOrWhiteSpace(surname) ? surname : throw new ArgumentNullException(nameof(surname));
            Patronymic = !string.IsNullOrWhiteSpace(patronymic) ? patronymic : throw new ArgumentNullException(nameof(patronymic));
            Birthday = birthday;
            Address = address;
        }

    }
}