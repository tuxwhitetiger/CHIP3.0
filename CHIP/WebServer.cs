using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CHIP
{
    internal class WebServer
    {
        public static HttpListener listener;
        public static string url = "http://+:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        /*
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "  </head>" +
        "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";
        */
        internal String face = "Happy face";
        internal bool newface = false;
        private Logger mylogger;

        public WebServer(Logger mylogger)
        {
            this.mylogger = mylogger;
        }

        public void run()
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            mylogger.Log("created web listener");
            listener.Prefixes.Add(url);
            mylogger.Log("added web url");
            listener.Start();
            mylogger.Log("Listening for connections on "+ url);

            // Handle requests
            Task listenTask = HandleIncomingConnections(mylogger,newface,face);
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }

        public static async Task HandleIncomingConnections(Logger mylogger,bool newface,String face)
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;


                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }
                //if a face was posted
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/SetFace"))
                {
                    mylogger.Log("web server got a post req");
                    Stream body = req.InputStream;
                    Encoding encoding = req.ContentEncoding;
                    mylogger.Log("stream and encoding built");
                    StreamReader reader = new System.IO.StreamReader(body, encoding);
                    mylogger.Log("reader built go reader go");
                    string s = reader.ReadToEnd();
                    mylogger.Log("recived this : " + s);

                    //dosomething with the data here
                    //expecting Faces=Pacman+face
                    //needs to be Pacman face
                    string output = s.Split('=')[1].Replace('+',' ');
                    mylogger.Log("reader output converted to this :"+ output);
                    face = output;
                    newface = true;
                }

                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                // Write the response info
                
                string pageData = "ERROR COULD NOT LOAD HTML";
                try
                {
                    pageData = String.Concat(File.ReadAllLines("./chip.html"));
                }catch (Exception ex)
                {
                    mylogger.Log(ex.Message);
                }
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        internal String getface()
        {
            
            newface = false;
            return face;
        }
    }
}
