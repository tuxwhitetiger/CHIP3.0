using Microsoft.VisualBasic.FileIO;
using SharpDX.DXGI;
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
        //public static string[] urls = { "http://+:8000/", "http://10.1.1.1:8000/", "http://10.0.0.14:8000/" };
        public static string[] urls = { "http://10.1.1.1:8000/"};
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
        public static String face = "Happy face";
        public static bool newface = false;
        private Logger mylogger;

        public static double temperature = 0;
        public static double voltagetotal = 0;
        public static double voltagecell1 = 0;
        public static double voltagecell2 = 0;
        public static double AmpTotal = 0;


        public WebServer(Logger mylogger)
        {
            this.mylogger = mylogger;
        }

        public void run()
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            mylogger.Log("created web listener");
            foreach (string url in urls)
            {
                listener.Prefixes.Add(url);
                mylogger.Log("added web url:"+url);
            }
            try
            {
                listener.Start();
                mylogger.Log("Listening for connections on urls");
            }catch(Exception ex)
            {
                mylogger.Log(ex.ToString());
            }
            // Handle requests
            Task listenTask = HandleIncomingConnections(mylogger);
            listenTask.GetAwaiter().GetResult();

            mylogger.Log("close");
            // Close the listener
            listener.Close();
        }

        public static async Task HandleIncomingConnections(Logger mylogger)
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                mylogger.Log("waiting for webrequest");
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();
                mylogger.Log("webrequest resived");
                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;
                mylogger.Log("HttpListenerRequest and HttpListenerResponse setup");

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
                if ((req.HttpMethod == "GET") && (req.Url.AbsolutePath == "/chip-style.css"))
                {
                    byte[] data = { };
                    try
                    {
                        mylogger.Log("try and load css");
                        data = File.ReadAllBytes("./chip-style.css");

                        mylogger.Log("css loaded");
                    }
                    catch (Exception ex)
                    {
                        mylogger.Log(ex.Message);
                    }
                    resp.ContentType = "text/css";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.Length;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                else if ((req.HttpMethod == "GET") && (req.Url.AbsolutePath.Contains("update-battery"))) {

                    String[] strings = req.Url.AbsolutePath.Split("?")[1].Split("&");
                    //Expecting temperature=" + x + "&voltagetotal=" + x + "&voltagecell1=" + x + "&voltagecell2=" + x + "&AmpTotal=" + x;
                    temperature = double.Parse(strings[0].Split("=")[1]);
                    voltagetotal = double.Parse(strings[1].Split("=")[1]);
                    voltagecell1 = double.Parse(strings[2].Split("=")[1]);
                    voltagecell2 = double.Parse(strings[3].Split("=")[1]);
                    AmpTotal = double.Parse(strings[4].Split("=")[1]);

                    byte[] data = { };
                    data = System.Text.Encoding.UTF8.GetBytes("done");
                    resp.ContentType = "text/html";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.Length;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);

                }
                else
                {
                    byte[] data = { };
                    try
                    {
                        mylogger.Log("try and load html");
                        data = File.ReadAllBytes("./chip.html");
                    }
                    catch (Exception ex)
                    {
                        mylogger.Log(ex.Message);
                    }
                    resp.ContentType = "text/html";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                
                // Write out to the response stream (asynchronously), then close it

                resp.Close();
            }
        }

        internal String getface()
        {
            newface = false;
            return face;
        }

        internal bool getnewface()
        {
            return newface;
        }
    }
}
