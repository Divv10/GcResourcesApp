using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Helpers
{
    public class XamlTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            Log.Error(message);
        }

        public override void WriteLine(string message)
        {
            Log.Error(message);
        }
    }
}
