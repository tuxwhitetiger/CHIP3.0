using Microsoft.VisualBasic.FileIO;
using RPiRgbLEDMatrix;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
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

        public static bool loggingEnabled = false;

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

        private static string textFacetext = " test ";
        private static string textFacespeed= "50";
        private static string textFacecolur = "ffffff";
        private static bool textFaceScroll = false;

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
            }
            catch(Exception ex)
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
                if (loggingEnabled) { 
                    mylogger.Log("waiting for webrequest"); 
                }
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();
                if (loggingEnabled)
                {
                    mylogger.Log("webrequest resived");
                }
                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;
                if (loggingEnabled)
                {
                    mylogger.Log("HttpListenerRequest and HttpListenerResponse setup");
                }

                //if a face was posted
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/SetFace"))
                {
                    if (loggingEnabled)
                    {
                        mylogger.Log("web server got a post req");
                    }
                    Stream body = req.InputStream;
                    Encoding encoding = req.ContentEncoding;
                    if (loggingEnabled)
                    {
                        mylogger.Log("stream and encoding built");
                    }
                    StreamReader reader = new System.IO.StreamReader(body, encoding);
                    if (loggingEnabled)
                    {
                        mylogger.Log("reader built go reader go");
                    }
                    string s = reader.ReadToEnd();
                    //if (loggingEnabled)
                    {
                        mylogger.Log("SetFace recived this : " + s);
                    }

                    //dosomething with the data here
                    //expecting Faces=Pacman+face
                    //needs to be Pacman face
                    string output = s.Split('=')[1].Replace('+', ' ');
                    //if (loggingEnabled)
                    {
                        mylogger.Log("reader output converted to this :" + output);
                    }

                    face = output;
                    newface = true;
                }
                else if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/SetTextFace"))
                {
                    if (loggingEnabled)
                    {
                        mylogger.Log("web server got a post req");
                    }
                    Stream body = req.InputStream;
                    Encoding encoding = req.ContentEncoding;
                    if (loggingEnabled)
                    {
                        mylogger.Log("stream and encoding built");
                    }
                    StreamReader reader = new System.IO.StreamReader(body, encoding);
                    if (loggingEnabled)
                    {
                        mylogger.Log("reader built go reader go");
                    }
                    string s = reader.ReadToEnd();
                    //if (loggingEnabled)
                    {
                        mylogger.Log("SetTextFace recived this : " + s);
                    }

                    //dosomething with the data here
                    //expecting ftext=hello+world+&speed=267&color=%23ffffff&leftToRight=ltr

                    string[] items = s.Split('&');

                    string output = items[0].Split('=')[1].Replace('+', ' ');
                    string speed = items[1].Split('=')[1];
                    string coluor = items[2].Split('=')[1].Replace("%23", "");
                    bool scroll = false;
                    if (items.Length > 3)
                    {
                        scroll = items[3].Split('=')[1].Equals("ltr");
                    }

                    //if (loggingEnabled)
                    {
                        mylogger.Log("reader output converted to this :" + output);
                    }
                    textFacetext = output;
                    textFacespeed = speed;
                    textFacecolur = coluor;
                    textFaceScroll = scroll;
                    face = "textFace";
                    newface = true;
                }
                else if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/rawrdata"))
                {
                    if (loggingEnabled)
                    {
                        mylogger.Log("web server got a post req");
                    }
                    Stream body = req.InputStream;
                    Encoding encoding = req.ContentEncoding;
                    if (loggingEnabled)
                    {
                        mylogger.Log("stream and encoding built");
                    }
                    StreamReader reader = new System.IO.StreamReader(body, encoding);
                    if (loggingEnabled)
                    {
                        mylogger.Log("reader built go reader go");
                    }
                    string s = reader.ReadToEnd();
                    
                    //if (loggingEnabled)
                    {
                       // mylogger.Log("rawrdata recived this : " + s);
                    }


                    //dosomething with the data here
                    //expecting ftext=hello+world+&speed=267&color=%23ffffff&leftToRight=ltr
                    //mylogger.Log("rawrdata recived this size : " + s.Length);
                    string[] colours = s.Split(',');
                    //mylogger.Log("colours size : " + s.Length);
                    int row = 0;
                    int col = 0;
                    Color[,] colorData = new Color[64,32];
                    //mylogger.Log("s1 length : " + colours[0].Length + " " + colours[0]);
                    int count = 0;
                    foreach (string s1 in colours)
                    {
                        mylogger.Log("row : " + row + " col:"+col + " count:"+count);
                        Color c = new Color(Convert.ToInt32(s1.Substring(1,2), 16), Convert.ToInt32(s1.Substring(3, 2), 16), Convert.ToInt32(s1.Substring(5,2), 16));
                        //mylogger.Log("c : " + c.ToString());
                        colorData[row,col] = c;
                        row++;
                        count++;
                        if(row == 64) {
                            row = 0;
                            col++;
                        }
                    }
                    mylogger.Log("pixelData size : " + colorData.Length);//2048
                    /*
                    string output = items[0].Split('=')[1].Replace('+', ' ');
                    string speed = items[1].Split('=')[1];
                    string coluor = items[2].Split('=')[1].Replace("%23", "");
                    bool scroll = false;
                    if (items.Length > 3)
                    {
                        scroll = items[3].Split('=')[1].Equals("ltr");
                    }

                    //if (loggingEnabled)
                    {
                        mylogger.Log("reader output converted to this :" + output);
                    }
                    textFacetext = output;
                    textFacespeed = speed;
                    textFacecolur = coluor;
                    textFaceScroll = scroll;
                    face = "textFace";
                    newface = true;
                    */
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
                        if (loggingEnabled)
                        {
                            mylogger.Log("try and load css");
                        }
                        data = File.ReadAllBytes("./chip-style.css");

                        if (loggingEnabled)
                        {
                            mylogger.Log("css loaded");
                        }
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
                    if (loggingEnabled)
                    {
                        mylogger.Log("battery trying to update with :" + req.Url.OriginalString);
                    }
                    String[] strings = req.Url.OriginalString.Split("?")[1].Split("&");
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
                        List<byte[]> bytes = new List<byte[]>();

                        if (loggingEnabled)
                        {
                            mylogger.Log("try and load html");
                        }
                        bytes.Add(File.ReadAllBytes("./chip.html"));
                        //bytes.Add(Encoding.ASCII.GetBytes("<div class=temp><p>Temperature :" + temperature + "</p></div>"));
                        //bytes.Add(Encoding.ASCII.GetBytes("<div class=volt><p>Voltage :" + voltagetotal + "</p></div>"));
                        //bytes.Add(Encoding.ASCII.GetBytes("<div class=amp><p>Amp :" + AmpTotal + "</p></div>"));
                        int Totallengeth = 0;
                        int lengthsofar = 0;
                        byte[][] mybytes = bytes.ToArray();
                        foreach (byte[] b in bytes)
                        {
                            Totallengeth += b.Length;
                        }
                        byte[] temp = new byte[Totallengeth];

                        for (int i = 0;i < bytes.Count; i++) {
                            if (i == 0)
                            {
                                Array.Copy(mybytes[i], temp, mybytes[i].Length);
                            }
                            else {
                                Array.Copy(mybytes[i], 0, temp, lengthsofar, mybytes[i].Length);

                            }
                            lengthsofar += mybytes[i].Length;
                        }


                        data = temp.ToArray();
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

        internal String GetText()
        {
            return textFacetext;
        }
        internal String GetTextFacespeed()
        {
            return textFacespeed;
        }
        internal String GetTextFaceColour()
        {
            return textFacecolur;
        }
        internal bool GetTextFaceScroll()
        {
            return textFaceScroll;
        }
    }
}