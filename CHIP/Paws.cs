using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;
using System.Threading.Tasks;
using System.Threading;

namespace CHIP
{
    internal class Paws
    {
        GpioController controller;
        Dictionary<int,string> fingers = new Dictionary<int,string>();
        public Paws()
        {
            fingers.Add(32, "LeftIndex");
            fingers.Add(34, "LeftMiddle");
            fingers.Add(36, "LeftRing");
            fingers.Add(38, "LeftLittle");

            fingers.Add(30, "RightIndex");
            fingers.Add(35, "RightMiddle");
            fingers.Add(37, "RightRing");
            fingers.Add(39, "RightLittle");

            controller = new GpioController();

            foreach (KeyValuePair<int, string> finger in fingers)
            {
                controller.OpenPin(finger.Key, PinMode.InputPullDown);
            }
        }

        public async Task Run() {
            foreach (KeyValuePair<int, string> finger in fingers)
            {
                controller.RegisterCallbackForPinValueChangedEvent(finger.Key, PinEventTypes.Falling | PinEventTypes.Rising, OnPinEvent);
            }
            
            await Task.Delay(Timeout.Infinite);
        }

        static void OnPinEvent(object sender, PinValueChangedEventArgs args)
        {
            int tiggerd = args.PinNumber;
            Console.WriteLine("TRIGGERED");
        }
    }
}
