using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Exceptions
{
    public class ProjectDomainException : Exception
    {
        public ProjectDomainException() : base()
        {

        }

        public ProjectDomainException(string msg) : base(msg)
        {

        }

        public ProjectDomainException(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}
