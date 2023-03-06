using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sw = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
              
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
              sw.WriteLine("Date Time: "+DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’"));
              sw.WriteLine("Message: "+ex.Message);
              sw.Close();
        }
          
    }
}
