using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO;
using Going.Boards.Interfaces;

namespace Going.Boards.LCD
{
    public class PiLCDEX : GoingBoard
    {
        #region Properties
        public override bool[] Input { get; } = new bool[4];
        public override bool[] Output { get; } = new bool[5];
        #endregion

        #region Member Variable
        IGpioPin[] Outs = new IGpioPin[5];
        IGpioPin[] Ins = new IGpioPin[4];
        #endregion

        #region Constructor
        public PiLCDEX()
        {
            Ins[0] = Pi.Gpio[P1.Pin38]; Ins[0].PinMode = GpioPinDriveMode.Input;
            Ins[1] = Pi.Gpio[P1.Pin40]; Ins[1].PinMode = GpioPinDriveMode.Input;
            Ins[2] = Pi.Gpio[P1.Pin15]; Ins[2].PinMode = GpioPinDriveMode.Input;
            Ins[3] = Pi.Gpio[P1.Pin16]; Ins[3].PinMode = GpioPinDriveMode.Input;

            Outs[0] = Pi.Gpio[P1.Pin29]; Outs[0].PinMode = GpioPinDriveMode.Output;
            Outs[1] = Pi.Gpio[P1.Pin31]; Outs[1].PinMode = GpioPinDriveMode.Output;
            Outs[2] = Pi.Gpio[P1.Pin32]; Outs[2].PinMode = GpioPinDriveMode.Output;
            Outs[3] = Pi.Gpio[P1.Pin33]; Outs[3].PinMode = GpioPinDriveMode.Output;

            Outs[4] = Pi.Gpio[P1.Pin22]; Outs[4].PinMode = GpioPinDriveMode.Output;
        }
        #endregion

        #region Method
        #region Begin
        public override void Begin()
        {
            Load();
            Out();
        }

        public void Begin(byte in_byte, byte out_byte)
        {
            Begin();
        }
        #endregion

        #region Load
        public override void Load()
        {
            for (int i = 0; i < 4; i++)
                Input[i] = Ins[i].Read();
        }
        #endregion

        #region Out
        public override void Out()
        {
            for (int i = 0; i < 5; i++)
                Outs[i].Write(Output[i]);
        }
        #endregion

        #endregion
    }
}
