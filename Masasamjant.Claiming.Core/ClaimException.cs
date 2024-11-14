using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masasamjant.Claiming
{
    public class ClaimException : Exception
    {
        public ClaimException(string message)
            : this(message, null) 
        { }

        public ClaimException(string message, Exception? innerException)
            : base(message, innerException) 
        { }
    }
}
