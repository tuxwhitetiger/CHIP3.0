using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace CHIP
{
    class Controller
    {
        public Socket socket;
        public int player = 0;
        public int up =0;
        public int down = 0;
        public int left = 0;
        public int right = 0;
        public int select = 0;
        public int start = 0;
        public int a = 0;
        public int b = 0;
        public int x = 0;
        public int y = 0;
        int failureCount = 0;
        public bool killme =false;

        public Controller(int playernumber) {
            player = playernumber;
        }
        public bool isalive() {
            return socket.Connected;
        }
        public void getupdate()
        {
            while (true){
                if (socket.Available > 0)
                {
                    failureCount = 0;
                    Byte[] requestBytes = Encoding.ASCII.GetBytes("update");
                    Byte[] bytesReceived = new Byte[256];
                    socket.Send(requestBytes, requestBytes.Length, 0);
                    int bytes = 0;
                    StringBuilder sb = new StringBuilder();
                    while (!sb.ToString().Contains("DONE"))
                    {
                        bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                        sb.Append(Encoding.ASCII.GetString(bytesReceived, 0, bytes));
                    }
                    string toProcess = sb.ToString();
                    //Console.WriteLine("incoming data:" + toProcess);
                    string[] updates = toProcess.Split("DONE");
                    //Console.WriteLine("updates count:" + updates.Length);
                    string[] data = updates[updates.Length - 2].Split(',');//get most resent update
                                                                           //Console.WriteLine("data count:" + data.Length);
                                                                           //Console.WriteLine(data.ToString());
                    if (data.Length < 10)
                    {
                        Console.WriteLine("not enough data");
                    }
                    else
                    {
                        up = Int32.Parse(data[0]);
                        down = Int32.Parse(data[1]);
                        left = Int32.Parse(data[2]);
                        right = Int32.Parse(data[3]);
                        select = Int32.Parse(data[4]);
                        start = Int32.Parse(data[5]);
                        a = Int32.Parse(data[6]);
                        b = Int32.Parse(data[7]);
                        x = Int32.Parse(data[8]);
                        y = Int32.Parse(data[9]);
                    }
                }
                else {
                    failureCount++;
                    if (failureCount > 10) {
                        Console.WriteLine("failureCount :"+ failureCount);
                        //killme = true;
                    }
                }
            }
        }

        internal String pullcurrentstate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("player:");
            sb.Append(player);
            sb.Append(", up:");
            sb.Append(up);
            sb.Append(", down:");
            sb.Append(down);
            sb.Append(", left:");
            sb.Append(left);
            sb.Append(", right:");
            sb.Append(right);
            sb.Append(", select:");
            sb.Append(select);
            sb.Append(", start:");
            sb.Append(start);
            sb.Append(", a:");
            sb.Append(a);
            sb.Append(", b:");
            sb.Append(b);
            sb.Append(", x:");
            sb.Append(x);
            sb.Append(", y:");
            sb.Append(y);

            return sb.ToString();

        }
    }
}
