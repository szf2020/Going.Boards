using Going.Boards;
using Going.Boards.LCD;
using System;

namespace RpiPLC
{
    class Program
    {
        static void Main(string[] args)
        {   
            GoingPLC engine = new GoingPLC();

            var LCDEX = new PiLCDEX();

            for (int i = 0; i < 8; i++)
            {
                LCDEX.InputMap.Add(i, "P" + (i + 1));
                LCDEX.OutputMap.Add(i, "P" + (i + 11));
            }
            engine.Boards.Add(LCDEX);
            engine.Start();
            
            Console.WriteLine("Ladder Engine Start");
            
            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }

            //engine.UnInitialize();
        }

    }

}