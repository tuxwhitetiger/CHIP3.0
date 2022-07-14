using System;
using rpi_rgb_led_matrix_sharp;
using System.Net;

namespace Speech.Recognition.Example
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var matrix = new RGBLedMatrix(32, 2, 1);
            var canvas = matrix.CreateOffscreenCanvas();

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
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "http";
                uriBuilder.Host = "webcode.me";
                uriBuilder.Path = "/";

                Uri uri = uriBuilder.Uri;

                WebRequest request = WebRequest.Create(uri);
                using WebResponse response = request.GetResponse();

                var headers = response.Headers;
                Console.WriteLine(headers);
            }
        }
    }
}