using System;

namespace Report.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    /// 
    public class ReportingDomainException: Exception
    {
        public ReportingDomainException()
        { }

        public ReportingDomainException(string message)
            : base(message)
        { }

        public ReportingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
