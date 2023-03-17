using Devinno.Extensions;
using Devinno.PLC.Ladder;
using Going.Boards.Chips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Going.Boards.Boards.Linked
{
    public class OUT40T : GoingBoard
    {
        #region Member Variable
        PCA9506 devO;
        byte DeviceID;
        #endregion

        #region Constructor
        public OUT40T(byte DeviceID = 0x21)
        {
            this.DeviceID = DeviceID;

            Hardwares = new IHardware[40];
            for (int i = 0; i < 40; i++) Hardwares[i] = new HardwareOutput($"OUT{i}");
        }
        #endregion

        #region Override
        #region Begin
        public override void Begin()
        {
            if (PLC != null)
            {
                devO = new PCA9506(DeviceID);

                devO.Setup();
                devO.PortMode(PCA9506.Port.Port0, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port1, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port2, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port3, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port4, PCA9506.PinMode.OUTPUT);

                Load();
                Out();
            }
        }
        #endregion
        #region End
        public override void End()
        {
        }
        #endregion

        #region Load
        public override void Load()
        {
 
        }
        #endregion
        #region Out
        public override void Out()
        {
            if (PLC != null && PLC.State == EngineState.RUN)
            {
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i] as HardwareOutput;

                    AddressInfo addr;
                    if (AddressInfo.TryParse(v.Address, out addr))
                    {
                        if (addr.Code == "P" && addr.Index >= 0 && addr.Index < PLC.P.Size) v.Value = PLC.P[addr.Index];
                        if (addr.Code == "M" && addr.Index >= 0 && addr.Index < PLC.M.Size) v.Value = PLC.M[addr.Index];
                    }
                }
            }
        }
        #endregion

        #region Update
        public override void Update()
        {
            if (PLC != null && PLC.State == EngineState.RUN)
            {
                byte[] values = new byte[5];
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i] as HardwareOutput;

                    var idx = i / 8;
                    var bit = i % 8;

                    values[idx].Bit(bit, v.Value);
                }

                devO.WritePort(PCA9506.Port.Port0, values[0]);
                devO.WritePort(PCA9506.Port.Port1, values[1]);
                devO.WritePort(PCA9506.Port.Port2, values[2]);
                devO.WritePort(PCA9506.Port.Port3, values[3]);
                devO.WritePort(PCA9506.Port.Port4, values[4]);
            }
        }
        #endregion
        #endregion
    }
}
