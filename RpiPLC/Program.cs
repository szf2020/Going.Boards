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
            Console.WriteLine($"PATH:{AppDomain.CurrentDomain.BaseDirectory}");

            Pi.Init<BootstrapWiringPi>();
            var plc = new GoingPLC();
            #region Pi16R
            /*
            var bd = new IO16R();
            for (int i = 0; i < 8; i++)
            {
                bd.Hardwares[$"OUT{i}"].Address = $"P{i+10}";
                bd.Hardwares[$"IN{i}"].Address = $"P{i}";
            }
            plc.Shields.Add(bd);
            */
            #endregion
            #region OUT40T
            /*
            var Link40T = new OUT40T(0x21);
            for (int i = 0; i < 40; i++)
            {
                Link40T.Hardwares[$"OUT{i}"].Address = $"P{i+40}";
            }
            plc.Shields.Add(Link40T);
            */
            #endregion
            #region IO80T
            /*
            var Link80T = new IO80T(0x21, 0x25);
            for (int i = 0; i < 40; i++)
            {
                Link80T.Hardwares[$"IN{i}"].Address = $"P{i + 40}";
                Link80T.Hardwares[$"OUT{i}"].Address = $"P{i + 80}";
                
            }
            plc.Shields.Add(Link80T);
            */
            #endregion
            #region OUT16R
            /*
            var Link16R = new OUT16R(0x21);
            for (int i = 0; i < 16; i++)
            {
                Link16R.Hardwares[$"OUT{i}"].Address = $"P{i + 20}";
            }
            plc.Shields.Add(Link16R);
            */
            #endregion
            #region SD8I
            /*
            var sd8i = new SD8I();
            for (int i = 0; i < 8; i++)
            {
                sd8i.Hardwares[$"IN{i}"].Address = $"P{i}";
            }
            plc.Shields.Add(sd8i);
            */
            #endregion
            #region SD8R
            /*
            var sd8r = new SD8R();
            for (int i = 0; i < 8; i++)
            {
                sd8r.Hardwares[$"OUT{i}"].Address = $"P{i+10}";
            }
            plc.Shields.Add(sd8r);
            */
            #endregion

            #region SD8IR
            
            var sd8ir = new SD8IR();
            for (int i = 0; i < 4; i++)
            {
                sd8ir.Hardwares[$"IN{i}"].Address = $"P{i}";
                sd8ir.Hardwares[$"OUT{i}"].Address = $"P{i+10}";
            }
            plc.Shields.Add(sd8ir);
            
            #endregion
            #region LcdEX
            /*
            var lcd = new LcdEX();
            for (int i = 0; i < 4; i++)
            {
                lcd.Hardwares[$"IN{i}"].Address = $"P{i}";                
            }
            for (int i = 0; i < 5; i++)
            {
                lcd.Hardwares[$"OUT{i}"].Address = $"P{i + 10}";
            }

            lcd.Hardwares[$"DAC0"].Address = $"D100";

            plc.Shields.Add(lcd);
            */
            #endregion
            plc.Start();

            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }
             
        }

    }

}