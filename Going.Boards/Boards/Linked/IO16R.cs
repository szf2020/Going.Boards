using Devinno.PLC.Ladder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Boards.Linked
{
    public class IO16R : GoingBoard
    {
        #region Const
        readonly P1[] PINS_I = { P1.Pin38, P1.Pin40, P1.Pin15, P1.Pin16, P1.Pin18, P1.Pin22, P1.Pin37, P1.Pin13 };
        readonly P1[] PINS_O = { P1.Pin29, P1.Pin31, P1.Pin32, P1.Pin33, P1.Pin36, P1.Pin11, P1.Pin12, P1.Pin35 };
        #endregion

        #region Member Variable
        IGpioPin[] IN;
        IGpioPin[] OUT;
        #endregion

        #region Constructor
        public IO16R()
        {
            Hardwares = new IHardware[16];
            for (int i = 0; i < 8; i++) Hardwares[i + 0] = new HardwareInput($"IN{i}");
            for (int i = 0; i < 8; i++) Hardwares[i + 8] = new HardwareOutput($"OUT{i}");

            IN = new IGpioPin[8];
            OUT = new IGpioPin[8];
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
                    IN[i] = Pi.Gpio[PINS_I[i]];
                    IN[i].PinMode = GpioPinDriveMode.Input;
                }

                for (int i = 0; i < 8; i++)
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
                for (int i = 0; i < 8; i++)
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
                for (int i = 0; i < 8; i++)
                {
                    var v = Hardwares[i + 8] as HardwareOutput;

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
                    var v = Hardwares[i] as HardwareInput;
                    if (IN[i] != null) v.Value = IN[i].Value;
                }

                for (int i = 0; i < 8; i++)
                {
                    var v = Hardwares[i + 8] as HardwareOutput;
                    if (OUT[i] != null) OUT[i].Value = v.Value;
                }
            }
        }
        #endregion
        #endregion
    }
}
