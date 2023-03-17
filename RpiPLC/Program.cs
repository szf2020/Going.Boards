using Going.Boards;
using Going.Boards.Shields;
using System;
using System.Linq;

namespace RpiPLC
{
    class Program
    {
        static void Main(string[] args)
        {
            var plc = new GoingPLC();
            var bdi = new SD8I();
            var bdo = new SD8R();
            var dici = bdi.Hardwares.ToDictionary(x => x.Name);
            var dico = bdo.Hardwares.ToDictionary(x => x.Name);
            for (int i = 0; i < 8; i++)
            {
                dici[$"IN{i}"].Address = $"P{i}";
                dico[$"OUT{i}"].Address = $"P{i + 10}";
            }
            plc.Shields.Add(bdi);
            plc.Shields.Add(bdo);
            plc.Start();

            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }
             
        }

    }

}