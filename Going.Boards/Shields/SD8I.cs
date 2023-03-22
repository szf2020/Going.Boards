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
    public class SD8I : GoingBoard
    {
        #region Const
        readonly P1[] PINS = { P1.Pin38, P1.Pin40, P1.Pin15, P1.Pin16, P1.Pin18, P1.Pin22, P1.Pin37, P1.Pin13 };
        #endregion

        #region Member Variable
        IGpioPin[] IN;
        #endregion

        #region Constructor
        public SD8I()
        {
            IN = new IGpioPin[8];

            var vs = new IHardware[8];
            for (int i = 0; i < 8; i++) vs[i] = new HardwareInput($"IN{i}");
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
                    IN[i] = Pi.Gpio[PINS[i]];
                    IN[i].PinMode = GpioPinDriveMode.Input;
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
                    if(IN[i] != null) v.Value = IN[i].Value;
                }
            }
        }
        #endregion
        #endregion
    }
}
