using System;

namespace Identity.Administration.Infrastructure.Exceptions
{
    public class IdentityDomainException: Exception
    {
        public IdentityDomainException()
        { }

        public IdentityDomainException(string message)
          : base(message)
        { }

        public IdentityDomainException(string message, Exception innerException)
          : base(message, innerException)
        { }
    }
}
