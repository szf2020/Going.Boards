using Devinno.PLC.Ladder;
using Going.Boards.Interfaces;
using System;
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

        public GoingPLC()
        {            
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
                    v.Out();
                }
            };
        }

        #region Method
        #region Initialize
        public void Initialize()
        {
            Pi.Init<BootstrapWiringPi>();

            foreach (var v in Boards) v.Begin();
        }
        #endregion
        #endregion
    }
}