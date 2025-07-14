using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class clsLogEvent
    {
        /// <summary>
        /// This method writes a log about the exception sent to it
        ///It converts the exeption into readable message and then writes it
        /// </summary>
        /// <param name="exception">The exception occured</param>
        /// <param name="logType">The logging type its possible to be (EventLogEntryType.Error , Information , warning ...) \n it's set by default to error.</param>
        public static void Log(Exception exception, EventLogEntryType logType = EventLogEntryType.Error)
        {
            String Source = "DVLD";
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, "Application");
            }

            String LogMessage = GetFormatedExceptionMessage(exception);

            EventLog.WriteEntry(Source, LogMessage);
        }

        static string GetFormatedExceptionMessage(Exception exception)
        {
            return $"____Exception Log____\n\n" +
                $"Time : {DateTime.Now}\n" +
                $"Message : {exception.Message}\n" +
                $"Inner exception: {(exception.InnerException != null ? exception.InnerException.Message : "N/A")}\n" +
                $"Source : {exception.Source} \n";
        }
    }


}

