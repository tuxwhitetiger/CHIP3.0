using System;
using rpi_rgb_led_matrix_sharp;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Speech.Recognition.Example
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var matrix = new RGBLedMatrix(32, 2, 1);
            var canvas = matrix.CreateOffscreenCanvas();
            String HOST = "127.0.0.1"; // The server's hostname or IP address
            int PORT = 65432;  // The port used by the server

            for (var i = 0; i < 1000; ++i)
            {
                for (var y = 0; y < canvas.Height; ++y)
                {
                    for (var x = 0; x < canvas.Width; ++x)
                    {
                        canvas.SetPixel(x, y, new Color(i & 0xff, x, y));
                    }
                }
                canvas.DrawCircle(canvas.Width / 2, canvas.Height / 2, 6, new Color(0, 0, 255));
                canvas.DrawLine(canvas.Width / 2 - 3, canvas.Height / 2 - 3, canvas.Width / 2 + 3, canvas.Height / 2 + 3, new Color(0, 0, 255));
                canvas.DrawLine(canvas.Width / 2 - 3, canvas.Height / 2 + 3, canvas.Width / 2 + 3, canvas.Height / 2 - 3, new Color(0, 0, 255));

                canvas = matrix.SwapOnVsync(canvas);




                IPHostEntry hostEntry = Dns.GetHostEntry(HOST);

                var ipe = new IPEndPoint(hostEntry.AddressList[0], PORT);
                using var socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

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

                Byte[] requestBytes = Encoding.ASCII.GetBytes("Get Face");
                Byte[] bytesReceived = new Byte[256];

                socket.Send(requestBytes, requestBytes.Length, 0);

                int bytes = 0;
                var sb = new StringBuilder();

                do
                {
                    bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                    sb.Append(Encoding.ASCII.GetString(bytesReceived, 0, bytes));
                } while (bytes > 0);

                Console.WriteLine(sb.ToString());



            }
        }
    }
}