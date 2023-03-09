using Devinno.PLC.Ladder;
using Going.Boards.LCD;
using System;
using System.Collections.Generic;

namespace Going.Boards.Interfaces
{
    public interface IGoingBoard
    {
        bool[] Input { get; }
        bool[] Output { get; }
        ushort[] DAOUT { get; }

        Dictionary<int, string> InputMap { get; }
        Dictionary<int, string> OutputMap { get; }

        void Begin(GoingPLC engine);
        void Load(GoingPLC engine);
        void Out(GoingPLC engine);
        
        void InputMapping(GoingPLC engine);
        void OutputMapping(GoingPLC engine);
    }

    public abstract class GoingBoard : IGoingBoard
    {
        public abstract bool[] Input { get; }
        public abstract bool[] Output { get; }
        public abstract ushort[] DAOUT { get; }

        public Dictionary<int, string> InputMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> OutputMap { get; } = new Dictionary<int, string>();

        public abstract void Begin(GoingPLC engine);
        public abstract void Load(GoingPLC engine);
        public abstract void Out(GoingPLC engine);

        public void InputMapping(GoingPLC engine)
        {
            foreach (var index in InputMap.Keys)
            {
                if (index >= 0 && index < Input.Length)
                {
                    var addr = InputMap[index];
                    var value = Input[index];
                    var idx = 0;

                    if (addr.StartsWith("P") && int.TryParse(addr.Substring(1), out idx) && idx >= 0 && idx < engine.P.Size) engine.P[idx] = value;
                    if (addr.StartsWith("M") && int.TryParse(addr.Substring(1), out idx) && idx >= 0 && idx < engine.M.Size) engine.M[idx] = value;
                }
            }
            var v = engine.D[100] & 0x0fff;
            DAOUT[0] = (ushort)v;
        }

        public void OutputMapping(GoingPLC engine)
        {
            foreach (var index in OutputMap.Keys)
            {
                if (index >= 0 && index < Output.Length)
                {
                    var addr = OutputMap[index];
                    var idx = 0;

                    if (addr.StartsWith("P") && int.TryParse(addr.Substring(1), out idx) && idx >= 0 && idx < engine.P.Size) Output[index] = engine.P[idx];
                    if (addr.StartsWith("M") && int.TryParse(addr.Substring(1), out idx) && idx >= 0 && idx < engine.M.Size) Output[index] = engine.M[idx];
                }
            }
        }
    }
}
