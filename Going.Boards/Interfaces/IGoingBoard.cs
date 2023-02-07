using Devinno.PLC.Ladder;
using System.Collections.Generic;

namespace Going.Boards.Interfaces
{
    public interface IGoingBoard
    {
        bool[] Input { get; }
        bool[] Output { get; }

        Dictionary<int, string> InputMap { get; }
        Dictionary<int, string> OutputMap { get; }

        void Load();
        void Out();
        void Begin();

        void InputMapping(LadderEngine engine);
        void OutputMapping(LadderEngine engine);
    }

    public abstract class GoingBoard : IGoingBoard
    {
        public abstract bool[] Input { get; }
        public abstract bool[] Output { get; }

        public Dictionary<int, string> InputMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> OutputMap { get; } = new Dictionary<int, string>();

        public abstract void Begin();
        public abstract void Load();
        public abstract void Out();

        public void InputMapping(LadderEngine engine)
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
        }

        public void OutputMapping(LadderEngine engine)
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
