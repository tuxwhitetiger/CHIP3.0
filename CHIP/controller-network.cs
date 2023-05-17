using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CHIP
{
    class controller_network
    {
        /// <summary>
        /// accsepts a controller assigns a port and waits for another to reconnect
        /// </summary>
        /// 
        Logger mylogger;
        Socket socket;
        public List<Controller> controllers = new List<Controller>();
        int controllercount = 1;

        public controller_network(Logger mylogger) {
            this.mylogger = mylogger;
            mylogger.Log("started socket");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mylogger.Log("Parse IPAddress");
            IPAddress ipAddress = IPAddress.Parse("10.1.1.1");
            mylogger.Log("localEndPoint created");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 65433);
            mylogger.Log("socket bound");
            socket.Bind(localEndPoint);
            mylogger.Log("start factory");
            Task.Factory.StartNew(checkForNewController, TaskCreationOptions.LongRunning);
            mylogger.Log("controller_network setup complete");
        }
        

        public void checkForNewController() {
            mylogger.Log("started listning");
            Console.WriteLine("started listning");
            socket.Listen(10);
            Socket client = socket.Accept();
            Controller c = new Controller(controllercount);
            mylogger.Log("controller created");
            Console.WriteLine("controller created");
            c.socket = client;
            mylogger.Log("socket set");
            Console.WriteLine("socket set");
            controllers.Add(c);
            controllercount++;
            Task.Factory.StartNew(c.getupdate, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(checkForNewController, TaskCreationOptions.LongRunning);
        }
    }
}
