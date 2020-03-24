using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SerialMonitorWPF
{
    public class LogWriter
    {
        private static readonly string UstiFolderPath;
        private static readonly string LogFilePath;

        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        static LogWriter()
        {
            UstiFolderPath = Path.Combine(
                Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)),
                "USTI");
            LogFilePath = Path.Combine(UstiFolderPath, "Log.txt");
        }

        public static void WriteDataToLog(string dataWh)
        {
            if (!Directory.Exists(UstiFolderPath))
            {
                Directory.CreateDirectory(UstiFolderPath);
            }

            Lock.EnterWriteLock();
            try
            {
                using (var fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                {
                    dataWh += "\n";
                    var dataAsByteArray = new UTF8Encoding(true).GetBytes(dataWh);
                    fs.Write(dataAsByteArray, 0, dataWh.Length);
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
    }
}
