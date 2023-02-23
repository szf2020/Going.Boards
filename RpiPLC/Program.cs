using Going.Boards;
using Going.Boards.Chips;
using Going.Boards.LCD;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RpiPLC
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*
            GoingPLC engine = new GoingPLC();
            engine.Initialize();

            
            var LCDEX = new PiLCDEX();
            for (int i = 0; i < 8; i++)
            {
                LCDEX.InputMap.Add(i, "P" + (i + 1));
                LCDEX.OutputMap.Add(i, "P" + (i + 11));
            }

            engine.Boards.Add(LCDEX);            
            
            engine.Start();
            
                        
            Console.WriteLine("Ladder Engine Start");
            */


            Pi.Init<BootstrapWiringPi>();
            MCP4725 Dev;
            Dev = new MCP4725(0x60);

            ushort n = 100;
            while (true)
            {
                Dev.WriteData(n);
                n += 100;
                if (n >= 600) n = 100;
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}