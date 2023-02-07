using Going.Boards;
using System;

namespace RpiPLC
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Engine
            GoingPLC engine = new GoingPLC();
            engine.Initialize();

            #region External Board
            var EXSD8I = new SD8I();
            var EXSD8R = new SD8R();
            for (int i = 0; i < 8; i++)
            {
                EXSD8I.InputMap.Add(i, "P" + (i + 1));
                EXSD8R.OutputMap.Add(i, "P" + (i + 11));
            }
            engine.Boards.Add(EXSD8I);
            engine.Boards.Add(EXSD8R);
            #endregion
            engine.Start();
            #endregion

            Console.WriteLine("Ladder Engine Start");

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}