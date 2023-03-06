using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
              string statusLine = "";
             statusLine=GetStatusLine(code);
              // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])

            string Date = DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’");
            headerLines.Add("Data: " + Date);
            headerLines.Add("ServerType: "+Configuration.ServerType);
              headerLines.Add("Content-Type: "+contentType);
              headerLines.Add("Content-Length: " + content.Length);
              if (code == StatusCode.Redirect)
              {
                    headerLines.Add("Location: " + redirectoinPath);
                    responseString = statusLine + "\r\n" + headerLines[0] + "\r\n" + headerLines[1] + "\r\n" + headerLines[2] + "\r\n" + headerLines[3]+"\r\n"+headerLines[4]+"\r\n"+"\r\n"+content;
              }
            // TODO: Create the request string
              else
              {
                    responseString = statusLine + "\r\n" + headerLines[0] + "\r\n" + headerLines[1] + "\r\n" + headerLines[2] + "\r\n" + headerLines[3] +"\r\n" + "\r\n" + content;
              }
             
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
              string msg = "";
              int c=Convert.ToInt32(code);
              string StatusLine = Configuration.ServerHTTPVersion + " " + c.ToString() + " ";
              if (code == StatusCode.OK)
                    msg = "OK";
              else if (code == StatusCode.BadRequest)
                    msg = "BadRequest";
              else if (code == StatusCode.InternalServerError)
                    msg = "InternalServerError";
              else if (code == StatusCode.NotFound)
                    msg = "NotFound";
              else {
                    if (code == StatusCode.Redirect)
                          msg = "Redirect";
                   }
              StatusLine += msg;
              return StatusLine;
        }
    }
}
