using System;
using System.IO;

namespace tk0wnz_indicators
{
    public static class Logger
    {
        private const string mLogFile = @"scripts\tk0wnz-indicators.log";

        public enum Level
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
        }

        public static void Clear()
        {
            File.WriteAllText(mLogFile, string.Empty);
        }

        public static void Log(Level level, string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(mLogFile, true))
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                streamWriter.WriteLine("[{0}] [{1, -5}] {2}", timestamp, level, message);
                streamWriter.Close();
            }
        }
    }
}