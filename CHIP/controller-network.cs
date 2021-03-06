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

        Socket socket;
        public List<Controller> controllers = new List<Controller>();
        int controllercount = 1;

        public controller_network() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("10.1.1.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 65433);
            socket.Bind(localEndPoint);
            Task.Factory.StartNew(checkForNewController, TaskCreationOptions.LongRunning);
        }

        public void checkForNewController() {
            Console.WriteLine("started listning");
            socket.Listen(10);
            Socket client = socket.Accept();
            Console.WriteLine("something sonnected");
            Controller c = new Controller(controllercount);
            Console.WriteLine("controller created");
            c.socket = client;
            Console.WriteLine("socket set");
            controllers.Add(c);
            controllercount++;
            Console.WriteLine("added to list");
            Task.Factory.StartNew(checkForNewController, TaskCreationOptions.LongRunning);
        }
    }
}
