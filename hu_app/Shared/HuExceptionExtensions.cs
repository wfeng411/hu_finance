using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace hu_app.Shared
{
    public static class HuExceptionExtensions
    {
        public static List<string> GetErrors(this Exception exception)
        {
            if (exception is HuException ex)
            {
                return ex.Errors ?? new List<string> { ex.Error };
            }
            else
            {
                var error = exception?.InnerException?.Message;
                if (string.IsNullOrWhiteSpace(error))
                {
                    error = exception?.Message;
                    if (string.IsNullOrWhiteSpace(error))
                    {
                        error = "Unknown";
                    }
                }
                return new List<string> { error };
            }
        }

        public static string GetPosition(this Exception e)
        {
            try
            {
                var stackTrace = new StackTrace(e, true);
                var stackFrame = stackTrace.GetFrames().FirstOrDefault(x => x.GetFileLineNumber() >= 1);
                if (stackFrame == null)
                {
                    return null;
                }
                var fileName = stackFrame.GetFileName();
                var fileLineNumber = stackFrame.GetFileLineNumber();
                //var method = stackFrame.GetMethod().Name;
                fileName = fileName.Split('\\').LastOrDefault();
                return $"{fileName}, line {fileLineNumber}";
            }
            catch
            {
                return null;
            }
        }
    }
}
