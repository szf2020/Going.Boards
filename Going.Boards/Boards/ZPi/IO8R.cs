using Devinno.PLC.Ladder;
using Devinno.Tools;
using Going.Boards.Chips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Boards.ZPi
{
    public class IO8R : GoingBoard
    {
        #region Const
        readonly P1[] PINS_I = { P1.Pin29, P1.Pin31, P1.Pin32, P1.Pin33 };//{ P1.Pin32, P1.Pin33, P1.Pin29, P1.Pin31 };
        readonly P1[] PINS_O = { P1.Pin15, P1.Pin16, P1.Pin37, P1.Pin13 }; //{ P1.Pin15, P1.Pin16, P1.Pin13 };
        #endregion

        #region Member Variable
        IGpioPin[] IN;
        IGpioPin[] OUT;

        byte DeviceID;
        #endregion

        #region Constructor
        public IO8R()
        {
            var vs = new IHardware[8];
            for (int i = 0; i < 4; i++) vs[i + 0] = new HardwareInput($"IN{i}");
            for (int i = 0; i < 4; i++) vs[i + 4] = new HardwareOutput($"OUT{i}");
            Hardwares = new HardwareList(vs);

            IN = new IGpioPin[4];
            OUT = new IGpioPin[4];
        }
        #endregion

        #region Override
        #region Begin
        public override void Begin()
        {
            if (PLC != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    IN[i] = Pi.Gpio[PINS_I[i]];
                    IN[i].PinMode = GpioPinDriveMode.Input;
                }

                for (int i = 0; i < 4; i++)
                {
                    OUT[i] = Pi.Gpio[PINS_O[i]];
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
            if (PLC != null && PLC.State == EngineState.RUN)
            {
                for (int i = 0; i < 4; i++)
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
                for (int i = 0; i < 4; i++)
                {
                    var v = Hardwares[i + 4] as HardwareOutput;

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
                for (int i = 0; i < 4; i++)
                {
                    var v = Hardwares[i] as HardwareInput;
                    v.Value = IN[i].Value;
                }

                for (int i = 0; i < 4; i++)
                {
                    var v = Hardwares[i + 4] as HardwareOutput;
                    OUT[i].Value = v.Value;
                }

            }
        }
        #endregion
        #endregion

    }
}
