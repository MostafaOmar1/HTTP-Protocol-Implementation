using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
             this.LoadRedirectionRules(redirectionMatrixPath);

              //TODO: initialize this.serverSocket
             this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
              IPEndPoint iep=new IPEndPoint(IPAddress.Any,portNumber);
              this.serverSocket.Bind(iep);
        }



        public void StartServer()
        {
              Console.WriteLine("Listening....");
            // TODO: Listen to connections, with large backlog.
              this.serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                  Socket clientSocket = this.serverSocket.Accept();
                  //Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);   
                  Thread newThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                  newThread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
              Socket clientSocket = (Socket)obj;
              clientSocket.ReceiveTimeout = 0;


            // TODO: receive requests in while true until remote client closes the socket.
              Console.WriteLine("Welcome to The Server");
            while (true)
            {
                try
                {
                    // TODO: Receive request
                      byte[] data = new byte[1024*1024];
                      int LenOfData = clientSocket.Receive(data);


                    // TODO: break the while loop if receivedLen==0
                      if (LenOfData == 0)
                            break;
                      string Cmsg = Encoding.ASCII.GetString(data, 0, LenOfData);


                    // TODO: Create a Request object using received request string
                      Request Qs = new Request(Cmsg);

                    // TODO: Call HandleRequest Method that returns the response
                      Response Rs=HandleRequest(Qs);
                    // TODO: Send Response back to client
                      string response_string = Rs.ResponseString;
                      byte[] Response = Encoding.ASCII.GetBytes(response_string);
                      clientSocket.Send(Response);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                      Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
             
              string physical_path="";
              StatusCode current = StatusCode.OK;
              string content = "";
            try
            {
                //  throw new Exception();
                //TODO: check for bad request 
                  //TODO: map the relativeURI in request to get the physical path of the resource.
                     if (request.ParseRequest()==false)
                   {
                        current=StatusCode.BadRequest;
                        physical_path = Configuration.RootPath + '\\' + Configuration.NotFoundDefaultPageName;
                        content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                        Response R = new Response(current, "text/html", content, physical_path);
                        return R;
                   }
                     physical_path = Configuration.RootPath + AddToPhyscial(request.relativeURI);

                //TODO: check for redirect
                  
                     string check = GetRedirectionPagePathIFExist(AddToPhyscial(request.relativeURI));
                   if (check !="")
                          {
                                current = StatusCode.Redirect;
                       
                         physical_path = check;
                        
                       content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                       
                       string location = Configuration.RedirectionRules[AddToPhyscial(request.relativeURI)];
                       Response response = new Response(current, "text/html", content, location);
                         return response;
                          }
                //TODO: check file exists
                  if (!File.Exists(physical_path))
                  {
                        current = StatusCode.NotFound;
                        physical_path = Configuration.RootPath + '\\' + Configuration.NotFoundDefaultPageName;
                        content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                        Response response = new Response(current, "text/html", content, physical_path);
                        return response;
                  }
                //TODO: read the physical file
                 // Create OK response
                  else
                  {
                        content = File.ReadAllText(physical_path);
                        Response response = new Response(current, "text/html", content, physical_path);
                        return response;
                  }

                
                
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
                  Logger.LogException(ex);
                  physical_path = Configuration.RootPath + '\\' + Configuration.InternalErrorDefaultPageName;
                  current = StatusCode.InternalServerError;
                  content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);

                  Response R = new Response(current, "text/html", content, physical_path);
                  return R;
            }
        }



        private string GetRedirectionPagePathIFExist(string relativePath)
        {
// using Configuration.RedirectionRules return the redirected page path if exists else returns empty
              string path = "";
              string temp1 = Configuration.RootPath + '\\' + Configuration.RedirectionDefaultPageName;
              if (Configuration.RedirectionRules.ContainsKey(relativePath))
              {
                    //System.Diagnostics.Process.Start(temp1);
                    string temp = Configuration.RedirectionRules[relativePath];
                    path = Configuration.RootPath +"\\"+ AddToPhyscial(temp);
                    return path;
              }
              else 
                    return string.Empty;

        }



        private string LoadDefaultPage(string defaultPageName)
        {
              string content = "";
              string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
              // TODO: check if filepath not exist log exception using Logger class and return empty string
              try
              {
                    if (File.Exists(filePath))
                    {
                          content=File.ReadAllText(filePath);
                          return content;
                    }
              }
              catch (Exception ex)
              {
                    Logger.LogException(ex);
              }
              // else read file and return its content
              return string.Empty;
        }


        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                  // then fill Configuration.RedirectionRules dictionary
                  string[] s;
                  StreamReader sr = new StreamReader(filePath);
                 while(sr.Peek()!=-1){
                       s = sr.ReadLine().Split(',');
                       Configuration.RedirectionRules.Add("\\"+s[0], s[1]);
                 }
                  
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                  Logger.LogException(ex);

                Environment.Exit(1);
            }
        }

        public string AddToPhyscial(String str )
        {
              string s = "";
              char c = '\\';
              
              for (int i = 0; i < str.Length; i++)
                    if (str[i] == '/')
                          s += c;
                    else
                          s += str[i];
              return s;
        }
    }
}
