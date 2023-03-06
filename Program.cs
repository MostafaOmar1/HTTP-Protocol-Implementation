using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
              CreateRedirectionRulesFile();


              //Start server
            // 1) Make server object on port 1000
              Server s = new Server(1000, "redirectionRules.txt");
            // 2) Start Server
              s.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
              try
              {
                    StreamWriter sw = new StreamWriter("redirectionRules.txt");
                   sw.WriteLine("aboutus.html,aboutus2.html");
                 
                    //int choise=1;
                    //string ToAdd="";
                    

                    //while (choise == 1)
                    //{
                    //      Console.Write("if you want to add more press 1 or (0 to quit):");
                    //      choise = Convert.ToInt32(Console.ReadLine());
                        
                    //      if (choise == 0)
                    //            break;
                    //      Console.Write("Enter the new rule:");
                    //      ToAdd = Console.ReadLine();
                    //      sw.WriteLine(ToAdd);
                    //}
                    sw.Close();
              }
              catch (Exception ex)
              {
                    Console.WriteLine(ex.Message);
              }
              // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
         
    }
}
