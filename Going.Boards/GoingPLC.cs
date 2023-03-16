using Devinno.Collections;
using Devinno.PLC.Ladder;
using Going.Boards.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Going.Boards
{
    public class GoingPLC : LadderEngine
    {
        #region Properties
        public EventList<GoingBoard> Shields { get; } = new EventList<GoingBoard>();
        #endregion

        #region Member Variable
        Thread th;
        #endregion

        #region Constructor
        public GoingPLC()
        {
            #region Shields.Changed
            Shields.Changed += (o, s) =>
            {
                foreach (var v in Shields) v.PLC = this;
            };
            #endregion

            #region Thread
            th = new Thread(() => {

                while (true)
                {
                    if(IsStart)
                    {
                        foreach (var v in Shields) v.Update();
                    }
                    Thread.Sleep(10);
                }
            
            }) { IsBackground = true };
            th.Start();
            #endregion
        }
        #endregion

        #region Override
        #region OnEngineStart
        public override void OnEngineStart()
        {
            if (IsStart)
            {
                foreach (var v in Shields) v.Begin();
            }
            base.OnEngineStart();
        }
        #endregion
        #region OnEngineStop
        public override void OnEngineStop()
        {
            if (IsStart)
            {
                foreach (var v in Shields) v.End();
            }
            base.OnEngineStop();
        }
        #endregion

        #region OnDeviceLoad
        public override void OnDeviceLoad()
        {
            if (IsStart)
            {
                foreach (var v in Shields) v.Load();
            }
            base.OnDeviceLoad();
        }
        #endregion
        #region OnDeviceOuput
        public override void OnDeviceOuput()
        {
            if (IsStart)
            {
                foreach (var v in Shields) v.Out();
            }
            base.OnDeviceOuput();
        }
        #endregion
        #endregion
    }
}
