using Going.Boards;
using Going.Boards.Boards.Linked;
using Going.Boards.Chips;
using Going.Boards.Shields;
using System;
using System.Globalization;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace RpiPLC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Pi.Init<BootstrapWiringPi>();

            var plc = new GoingPLC();
            var bd = new OUT40T(0x21);
            for (int i = 0; i < 40; i++)
            {
                bd.Hardwares[$"OUT{i}"].Address = $"P{i}";
            }
            plc.Shields.Add(bd);
            plc.Start();

            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }
             
        }

    }

}