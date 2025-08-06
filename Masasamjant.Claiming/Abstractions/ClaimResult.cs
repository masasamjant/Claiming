using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masasamjant.Claiming
{
    public enum ClaimResult : int
    {
        /// <summary>
        /// Item attempt to claim not found.
        /// </summary>
        NotFound  = 0,

        /// <summary>
        /// Claim to item was approved.
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Claim to item was denied.
        /// </summary>
        Denied = 2
    }
}
