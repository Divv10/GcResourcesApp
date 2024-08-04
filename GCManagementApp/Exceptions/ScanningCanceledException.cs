using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Exceptions
{
    public class ScanningCanceledException : Exception
    {
        public ScanningCanceledException()
        {
        }

        public ScanningCanceledException(string message)
            : base(message)
        {
        }

        public ScanningCanceledException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
