using Going.Boards;
using Going.Boards.Boards.Core;
using Going.Boards.Boards.Linked;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RpiPLC
{
    class Program
    {
        static void Main(string[] args)
        {
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
            var Link80T = new IO80T(0x21, 0x25);
            for (int i = 0; i < 40; i++)
            {
                Link80T.Hardwares[$"IN{i}"].Address = $"P{i + 40}";
                Link80T.Hardwares[$"OUT{i}"].Address = $"P{i + 80}";                
            }
            plc.Shields.Add(Link80T);
            
            #endregion
            #region OUT16CH
            /*
            var Link16R = new OUT16CH(0x22);
            
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
            /*
            var sd8ir = new SD8IR();
            for (int i = 0; i < 4; i++)
            {
                sd8ir.Hardwares[$"IN{i}"].Address = $"P{i}";
                sd8ir.Hardwares[$"OUT{i}"].Address = $"P{i+10}";
            }
            plc.Shields.Add(sd8ir);
            */
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
            #region PILCD
            /*
            var lcd = new PILCD();
            for (int i = 0; i < 4; i++)
            {
                lcd.Hardwares[$"IN{i}"].Address = $"P{i}";
            }
            
            for (int i = 0; i < 3; i++)
            {
                lcd.Hardwares[$"OUT{i}"].Address = $"P{i + 10}";
            }
            
            plc.Shields.Add(lcd);
            */
            #endregion
            #region ZPiIO8R

            var Zero8r = new ZPiIO8R();


            for (int i = 0; i < 4; i++)
            {
                Zero8r.Hardwares[$"IN{i}"].Address = $"P{i}";
            }

            for (int i = 0; i < 4; i++)
            {
                Zero8r.Hardwares[$"OUT{i}"].Address = $"P{i + 10}";
            }

            plc.Shields.Add(Zero8r);

            #endregion

            plc.Start();

            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }
             
        }

    }

}