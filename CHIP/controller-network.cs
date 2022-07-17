using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CHIP
{
    class controller_network
    {
        /// <summary>
        /// accsepts a controller assigns a port and waits for another to reconnect
        /// </summary>
        /// 
        String HOST = "localhost"; // The server's hostname or IP address
        int PORT = 65433;  // The port used by the server
        int addressNumber = 1;
        Socket socket;
        int nextPORT = 65434;
        public List<Controller> controllers = new List<Controller>();
        int controllercount = 1;

        public controller_network() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            socket.Bind(localEndPoint);
            socket.Listen(100);
            Task.Factory.StartNew(checkForNewController, TaskCreationOptions.LongRunning);
        }
        public void checkForNewController() {
            while (true)
            {
                socket.Listen(100);
                Socket client = socket.Accept();
                Controller c = new Controller(controllercount);
                c.socket = client;
                controllers.Add(c);
                controllercount++;
            }
        }
    }
}
