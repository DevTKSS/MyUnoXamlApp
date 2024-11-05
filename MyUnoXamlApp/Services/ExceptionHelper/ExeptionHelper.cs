using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnoXamlApp.Services.ExceptionHelper;
public static class ExceptionHelper
{
    public static string HandleException(Exception exception)
    {
        if (exception == null) { return string.Empty; }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Exception Message: " + exception.GetType().Name);
            sb.AppendLine("Exception Message: " + exception.Message);
            GetTrace(exception, ref sb, 0);
            if (exception.InnerException != null)
            {
                sb.AppendLine("InnerException Type: " + exception.InnerException.GetType().Name);
                sb.AppendLine("InnerException Message: " + exception.InnerException.Message);
                GetTrace(exception.InnerException, ref sb, 1);
            }
            sb.AppendLine("Source: " + exception.Source);

            return sb.ToString();
        }

    }

    private static void GetTrace(Exception exception, ref StringBuilder sb, int currentIntentLevel)
    {
        StackTrace st = new StackTrace(exception, true);
        string stackIndent = new string(' ', currentIntentLevel * 2);
        for (int i = 0; i < st.FrameCount; i++)
        {
            StackFrame? sf = st.GetFrame(i);
            if (sf != null)
            {
                var method = sf.GetMethod();
                var fileName = sf.GetFileName();
                var lineNumber = sf.GetFileLineNumber();
                sb.AppendLine();
                sb.AppendFormat(stackIndent + " Method: {0}", method != null ? method.ToString() : "N/A");
                sb.AppendFormat(stackIndent + " File: {0}", fileName ?? "N/A");
                sb.AppendFormat(stackIndent + " Line Number: {0}", lineNumber != 0 ? lineNumber.ToString() : "N/A");
                stackIndent += "  ";
            }
        }
    }
}
