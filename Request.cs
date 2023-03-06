using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
         
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
           
            this.requestString = requestString;

            headerLines = new Dictionary<string, string>();
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
              
            //TODO: parse the receivedRequest using the \r\n delimeter   
              requestLines = this.requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
              if (requestLines.Length < 4)
                    return false;
                // Parse Request line
              // Validate blank line exists
              // Load header lines into HeaderLines dictionary
              if ((ParseRequestLine() == false) || (LoadHeaderLines() == false)||(ValidateBlankLine(headerLines.Count+1) == false))
                    return false;
              else
                  return true;
        }


        private bool ParseRequestLine()
        {
              string []RequestLineOnly = requestLines[0].Split(' ');
              if (GetMethod(RequestLineOnly[0]) == false)
                    return false;
              relativeURI = cutURI(requestLines[0]);
              if (!ValidateIsURI(relativeURI))
                    return false;

              int StartAfterURI = RequestLineOnly[0].Length+ (relativeURI.Length) + 2;
              if (Checkhttp(requestLines[0].Substring(StartAfterURI)) == false)
                    return false;

              
              return true;
        }



        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }



        private bool LoadHeaderLines()
        {
              
              List<string> actual_Header=new List<string>();
              for (int i = 1; i < requestLines.Length; i++)
              {
                    if (i == requestLines.Length - 1)
                          break;
                    else if (requestLines[i] != "" && requestLines[i + 1] == "")
                    {
                          actual_Header.Add(requestLines[i]);
                         
                          break;
                    }
                    else
                    {
                          actual_Header.Add(requestLines[i]);
                    }
              }

              
                   
              for (int i = 0; i < actual_Header.Count; i++)
              {
                    string[] temp = actual_Header[i].Split(':');
                    headerLines.Add(temp[0]+":", temp[1]);
              }
             
                    if (!headerLines.ContainsKey("Host:")) 
                    return false;

                    return true;
        }



        private bool ValidateBlankLine(int index)
        {
              if (requestLines[index] != "")
                    return false;
              return true;
        }


        public bool GetMethod(string s)
        {
              if (s == "GET")
                    method = RequestMethod.GET;
              else if (s == "POST")
                    method = RequestMethod.POST;
              else if (s == "HEAD")
                    method = RequestMethod.HEAD;
              else
                    return false;
        return true;
        }


        public string cutURI(string s)
        {
              string s2="";
              if (method == RequestMethod.GET)
              {
                    for (int i = 4; i < s.Length; i++)
                    {
                          if (s[i] == ' ')
                          {
                                break;
                          }
                          else
                                s2 += s[i];

                    }
              }
              if (method == RequestMethod.POST || method == RequestMethod.HEAD)
              {
                    for (int i = 5; i < s.Length; i++)
                    {
                          if (s[i] == ' ')
                          {
                                break;
                          }
                          else
                                s2 += s[i];

                    }
              }
                    
                return s2;
        }


        public bool Checkhttp( string h)
        {
              string name = h.Substring(0, 5);
              if (name != "HTTP/")
                    return false;

              string version = h.Substring(5);
              if (version == "1.0")
                    httpVersion = HTTPVersion.HTTP10;
              else if (version == "1.1")
                    httpVersion = HTTPVersion.HTTP11;
              else if (version == "0.9")
                    httpVersion = HTTPVersion.HTTP09;
              else
                    return false;
              return true;
        }
      

    

    }
}
