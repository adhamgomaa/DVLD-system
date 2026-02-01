using DVLD_Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Diagnostics;

namespace DVLD_Presentation.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;
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
