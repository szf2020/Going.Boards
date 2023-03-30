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
    public class OUT16R : GoingBoard
    {
        #region Member Variable
        MCP23017 devO;
        byte DeviceID;
        #endregion

        #region Constructor
        public OUT16R(byte DeviceID = 0x21)
        {
            this.DeviceID = DeviceID;

            var vs = new IHardware[16];
            for (int i = 0; i < 16; i++) vs[i] = new HardwareOutput($"OUT{i}");
            Hardwares = new HardwareList(vs);
        }
        #endregion

        #region Override 
        #region Begin
        public override void Begin()
        {
            if (PLC != null)
            {
                devO = new MCP23017(DeviceID);

                devO.Setup();
                devO.PortMode(MCP23017.Port.A, MCP23017.PinMode.OUTPUT);
                devO.PortMode(MCP23017.Port.B, MCP23017.PinMode.OUTPUT);

               //Load();
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
                for (int i = 0; i < 16; i++)
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
                byte[] values = new byte[2];
                for (int i = 0; i < 16; i++)
                {
                    var v = Hardwares[i] as HardwareOutput;

                    var idx = i / 8;
                    var bit = i % 8;

                    values[idx].Bit(bit, v.Value);
                }

                devO.WritePort(MCP23017.Port.A, values[0]);
                devO.WritePort(MCP23017.Port.B, values[1]);
            }
        }
        #endregion
        #endregion
    }
}
