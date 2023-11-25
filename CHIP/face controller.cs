using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;

namespace CHIP
{
    internal class face_controller
    {
        Face face;
        mynetwork net;
        faces currentface;
        WebServer webserver;
        string nextFace = "happy face";
        string lastFace = "happy face";

        public face_controller(Logger mylogger) {
            
            mylogger.Log("starting mynetwork");
            net = new mynetwork(mylogger);
            mylogger.Log("starting webserver");
            webserver = new WebServer(mylogger);
            //mylogger.Log("starting controller_network");
            //cnet = new controller_network(mylogger);
            //controllerstokill = new List<Controller>();
            mylogger.Log("starting CalanderClock");
            //clock = new CalanderClock(net);
            mylogger.Log("net.connect");
            net.connect();
            face = new Face();
            face.load(mylogger,net);



            Task task = Task.Factory.StartNew(() => { while (true) { Update(); } });
            Task task2 = Task.Factory.StartNew(() => { while (true) { featch(); } });
            Task task3 = Task.Factory.StartNew(() => { while (true) { webserver.run(); } });

        }

        public void Update()
        {
            
            face.update(currentface);
        }

        public void featch() {

            lastFace = nextFace;
            nextFace = net.getFace();
            if (lastFace.Equals(nextFace))
            {
                processFace(nextFace);
            }
            if (webserver.newface)
            {
                processFace(webserver.getface());
            }
        }

        public void processFace(String face) {
            switch (face) // need to make this async
            {
                case "Sad face": currentface = faces.sad; break;
                case "Happy face": currentface = faces.happy; break;
                case "Angry face": currentface = faces.Angry; break;
                case "What face": currentface = faces.What; break;
                case "Flag face": currentface = faces.Flag; break;
                case "Gif face": currentface = faces.Gif; break;
                case "Shock face": currentface = faces.Oh; break;
                case "Snake face": currentface = faces.snake; break;
                case "Overheat face": currentface = faces.Overheat; break;
                case "Cwood face": currentface = faces.cwood; break;
                case "Lowbatt face": currentface = faces.lowbatt; break;
                case "Pacman face": currentface = faces.pacman; break;
                case "Matrix face": currentface = faces.matrix; break;
                case "8 Ball Face": currentface = faces.eightball; break;
                case "SHAKE BALL": currentface = faces.eightball; break;
                case "HALLOWEEN FACE": currentface = faces.Halloween; break;
                case "LOVE FACE": currentface = faces.Love; break;
                case "textTest face": currentface = faces.textTest; break;
                case "DvD face": currentface = faces.DvDBounce; break;
                case "matrix rain": currentface = faces.matrixRain; break;
            }
        }

    }
}
