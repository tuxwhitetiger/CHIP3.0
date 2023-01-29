using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CHIP
{
    class mynetwork
    {
        String HOST = "localhost"; // The server's hostname or IP address
        int PORT = 65432;  // The port used by the server
        int addressNumber = 1;
        Socket socket;

        public mynetwork()
        {


        }

        public void connect() {
            IPHostEntry hostEntry = Dns.GetHostEntry(HOST);
            IPEndPoint ipe = new IPEndPoint(hostEntry.AddressList[addressNumber], PORT);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            if (socket.Connected)
            {
                Console.WriteLine("Connection established");
            }
            else
            {
                Console.WriteLine("Connection failed");
                return;
            }
        }

        public String GetGifData(String fileName) {
            Byte[] requestBytes = Encoding.ASCII.GetBytes("Pull Gif:"+fileName);
            Byte[] bytesReceived = new Byte[1000];
            socket.Send(requestBytes, requestBytes.Length, 0);
            int bytes = 0;
            StringBuilder sb = new StringBuilder();

            while (!sb.ToString().Contains("DONE"))
            {
                bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                sb.Append(Encoding.ASCII.GetString(bytesReceived, 0, bytes));
            }
            return sb.ToString();
        }

        public String getFace()
        {
            Byte[] requestBytes = Encoding.ASCII.GetBytes("Get Face");
            Byte[] bytesReceived = new Byte[256];
            socket.Send(requestBytes, requestBytes.Length, 0);
            int bytes = 0;
            StringBuilder sb = new StringBuilder();
            bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
            sb.Append(Encoding.ASCII.GetString(bytesReceived, 0, bytes));
            return sb.ToString();
        }

        public String getAlarm()
        {
            Byte[] requestBytes = Encoding.ASCII.GetBytes("Get Alarm");
            Byte[] bytesReceived = new Byte[256];
            socket.Send(requestBytes, requestBytes.Length, 0);
            int bytes = 0;
            StringBuilder sb = new StringBuilder();
            bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
            sb.Append(Encoding.ASCII.GetString(bytesReceived, 0, bytes));
            return sb.ToString();
        }


    }
}
