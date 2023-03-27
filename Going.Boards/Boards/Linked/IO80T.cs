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
    public class IO80T : GoingBoard
    {
        #region Member Variable
        PCA9506 devO;
        PCA9506 devI;
        byte DeviceInputID;
        byte DeviceOutputID;
        #endregion

        #region Constructor
        public IO80T(byte DeviceInputID = 0x21, byte DeviceOutputID = 0x25)
        {
            this.DeviceInputID = DeviceInputID;
            this.DeviceOutputID = DeviceOutputID;

            var vs = new IHardware[80];
            for (int i = 0; i < 40; i++) vs[i + 0] = new HardwareInput($"IN{i}");
            for (int i = 0; i < 40; i++) vs[i + 40] = new HardwareOutput($"OUT{i}");
            Hardwares = new HardwareList(vs);
        }
        #endregion

        #region Override
        #region Begin
        public override void Begin()
        {
            if (PLC != null)
            {
                devO = new PCA9506(DeviceOutputID);
                devI = new PCA9506(DeviceInputID);

                devO.Setup();
                devO.PortMode(PCA9506.Port.Port0, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port1, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port2, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port3, PCA9506.PinMode.OUTPUT);
                devO.PortMode(PCA9506.Port.Port4, PCA9506.PinMode.OUTPUT);

                devI.Setup();
                devI.PortMode(PCA9506.Port.Port0, PCA9506.PinMode.INPUT);
                devI.PortMode(PCA9506.Port.Port1, PCA9506.PinMode.INPUT);
                devI.PortMode(PCA9506.Port.Port2, PCA9506.PinMode.INPUT);
                devI.PortMode(PCA9506.Port.Port3, PCA9506.PinMode.INPUT);
                devI.PortMode(PCA9506.Port.Port4, PCA9506.PinMode.INPUT);

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
            if (PLC != null && PLC.State == EngineState.RUN)
            {
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i] as HardwareInput;

                    AddressInfo addr;
                    if (AddressInfo.TryParse(v.Address, out addr))
                    {
                        if (addr.Code == "P" && addr.Index >= 0 && addr.Index < PLC.P.Size) PLC.P[addr.Index] = v.Value;
                        if (addr.Code == "M" && addr.Index >= 0 && addr.Index < PLC.M.Size) PLC.M[addr.Index] = v.Value;
                    }
                }
            }
        }
        #endregion
        #region Out
        public override void Out()
        {
            if (PLC != null && PLC.State == EngineState.RUN)
            {
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i + 40] as HardwareOutput;

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
                
                byte[] valuesI = new byte[5];
                valuesI[0] = devI.ReadPort(PCA9506.Port.Port0);
                valuesI[1] = devI.ReadPort(PCA9506.Port.Port1);
                valuesI[2] = devI.ReadPort(PCA9506.Port.Port2);
                valuesI[3] = devI.ReadPort(PCA9506.Port.Port3);
                valuesI[4] = devI.ReadPort(PCA9506.Port.Port4);
                
                
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i] as HardwareInput;

                    var idx = i / 8;
                    var bit = i % 8;

                    v.Value = valuesI[idx].Bit(bit);
                }
                
                
                byte[] valuesO = new byte[5];
                for (int i = 0; i < 40; i++)
                {
                    var v = Hardwares[i + 40] as HardwareOutput;

                    var idx = i / 8;
                    var bit = i % 8;

                    valuesO[idx].Bit(bit, v.Value);
                }

                devO.WritePort(PCA9506.Port.Port0, valuesO[0]);
                devO.WritePort(PCA9506.Port.Port1, valuesO[1]);
                devO.WritePort(PCA9506.Port.Port2, valuesO[2]);
                devO.WritePort(PCA9506.Port.Port3, valuesO[3]);
                devO.WritePort(PCA9506.Port.Port4, valuesO[4]);
                
            }
        }
        #endregion
        #endregion
    }
}
