using Devinno.Communications.TextComm.TCP;
using Devinno.Data;
using Devinno.PLC.Ladder;
using Going.Boards.Interfaces;
using System.Collections.Generic;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Going.Boards
{
    public class GoingPLC : LadderEngine
    {
        #region Properties 
        public List<IGoingBoard> Boards { get; } = new List<IGoingBoard>();
        #endregion

        TextCommTCPSlave comm;

        public GoingPLC()
        {
            Pi.Init<BootstrapWiringPi>();

            #region Communication
            comm = new TextCommTCPSlave { DisconnectCheckTime = 3000 };
            comm.Start();
            comm.MessageRequest += (o, s) => {

                if (s.Command == 10)
                {
                    try
                    {
                        var v = Serialize.JsonDeserialize<List<Shield>>(s.RequestMessage);
                        if (v != null && v.Count > 0)
                        {

                        }
                    }
                    catch { }
                }

            };
            #endregion
            #region PLC
            DeviceLoad += (o, s) =>
            {
                foreach (var v in Boards)
                {
                    v.Load();
                    v.InputMapping(this);                    
                }
            };

            DeviceOuput += (o, s) =>
            {
                foreach (var v in Boards)
                {
                    v.OutputMapping(this);
                    v.DAOutput(this);
                    v.Out();
                }
            };
            #endregion
        }

        #region Method
        #region Initialize
        public void Initialize()
        {
            foreach (var v in Boards) v.Begin();            
        }
        #endregion
        #endregion
    }

    class Shield
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}