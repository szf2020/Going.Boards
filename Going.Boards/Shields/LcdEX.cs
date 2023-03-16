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

namespace Going.Boards.Shields
{
    public class LcdEX : GoingBoard
    {
        #region Const
        readonly P1[] PINS_I = { P1.Pin38, P1.Pin40, P1.Pin15, P1.Pin16 };
        readonly P1[] PINS_O = { P1.Pin29, P1.Pin31, P1.Pin32, P1.Pin33, P1.Pin22 };
        #endregion

        #region Member Variable
        IGpioPin[] IN;
        IGpioPin[] OUT;
        MCP4725 MCP;
        #endregion

        #region Constructor
        public LcdEX()
        {
            Hardwares = new IHardware[10];
            for (int i = 0; i < 4; i++) Hardwares[i + 0] = new HardwareInput($"IN{i}");
            for (int i = 0; i < 5; i++) Hardwares[i + 4] = new HardwareOutput($"OUT{i}");
            Hardwares[9] = new HardwareDAC($"DAC0");

            IN = new IGpioPin[4];
            OUT = new IGpioPin[5];
            MCP = new MCP4725(0x60);
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

                for (int i = 0; i < 5; i++)
                {
                    OUT[i] = Pi.Gpio[PINS_O[i]];
                    OUT[i].PinMode = GpioPinDriveMode.Output;
                }

                Load();
                Out();

                MCP.Setup();
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
            if (PLC != null)
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
            if (PLC != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    var v = Hardwares[i + 4] as HardwareOutput;

                    AddressInfo addr;
                    if (AddressInfo.TryParse(v.Address, out addr))
                    {
                        if (addr.Code == "P" && addr.Index >= 0 && addr.Index < PLC.P.Size) v.Value = PLC.P[addr.Index];
                        if (addr.Code == "M" && addr.Index >= 0 && addr.Index < PLC.M.Size) v.Value = PLC.M[addr.Index];
                    }
                }

                {
                    var v = Hardwares[9] as HardwareDAC;

                    AddressInfo addr;
                    if (AddressInfo.TryParse(v.Address, out addr))
                    {
                        if (addr.Code == "D" && addr.Index >= 0 && addr.Index < PLC.D.Size) v.Value = PLC.D[addr.Index];
                    }
                }
            }
        }
        #endregion

        #region Update
        public override void Update()
        {
            if (PLC != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    var v = Hardwares[i] as HardwareInput;
                    v.Value = IN[i].Value;
                }

                for (int i = 0; i < 5; i++)
                {
                    var v = Hardwares[i + 4] as HardwareOutput;
                    OUT[i].Value = v.Value;
                }

                {
                    var v = Hardwares[9] as HardwareDAC;
                    var val = Convert.ToUInt16(MathTool.Constrain(v.Value, 0, 4095));
                    MCP.WriteDAC(val);
                }
            }
        }
        #endregion
        #endregion
    }
}
