using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLogger
    {
        public static void LoggingAllExepctions(string message, EventLogEntryType type)
        {
            string nameOfProgram = "DVLD";
            if (!EventLog.SourceExists(nameOfProgram))
            {
                EventLog.CreateEventSource(nameOfProgram, "Application");
            }

            EventLog.WriteEntry(nameOfProgram, message, type);
        }
    }
}
