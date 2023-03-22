using Devinno.PLC.Ladder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Shields
{
    public class SD8R : GoingBoard
    {
        #region Const
        readonly P1[] PINS = { P1.Pin29, P1.Pin31, P1.Pin32, P1.Pin33, P1.Pin36, P1.Pin11, P1.Pin12, P1.Pin35 };
        #endregion

        #region Member Variable
        IGpioPin[] OUT;
        #endregion

        #region Constructor
        public SD8R()
        {
            OUT = new IGpioPin[8];

            var vs = new IHardware[8];
            for (int i = 0; i < 8; i++) vs[i] = new HardwareOutput($"OUT{i}");
            Hardwares = new HardwareList(vs);
        }
        #endregion

        #region Override
        #region Begin
        public override void Begin()
        {
            if (PLC != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    OUT[i] = Pi.Gpio[PINS[i]];
                    OUT[i].PinMode = GpioPinDriveMode.Output;
                }

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
                for (int i = 0; i < 8; i++)
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
                for (int i = 0; i < 8; i++)
                {
                    var v = Hardwares[i] as HardwareOutput;
                    if (OUT[i] != null) OUT[i].Value = v.Value;
                }
            }
        }
        #endregion
        #endregion
    }
}
