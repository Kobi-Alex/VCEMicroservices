using System;

namespace Report.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    /// 
    public class ReportDomainException: Exception
    {
        public ReportDomainException()
        { }

        public ReportDomainException(string message)
            : base(message)
        { }

        public ReportDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
