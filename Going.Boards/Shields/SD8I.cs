using Going.Boards.Interfaces;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards
{
    public class SD8I : GoingBoard
    {
        #region Properties
        public override bool[] Input { get; } = new bool[8];
        public override bool[] Output { get; } = new bool[8];
        #endregion

        #region Member Variable
        IGpioPin[] Outs = new IGpioPin[8];
        IGpioPin[] Ins = new IGpioPin[8];
        #endregion

        #region Constructor
        public SD8I()
        {          
            Ins[0] = Pi.Gpio[P1.Pin38]; Ins[0].PinMode = GpioPinDriveMode.Input;
            Ins[1] = Pi.Gpio[P1.Pin40]; Ins[1].PinMode = GpioPinDriveMode.Input;
            Ins[2] = Pi.Gpio[P1.Pin15]; Ins[2].PinMode = GpioPinDriveMode.Input;
            Ins[3] = Pi.Gpio[P1.Pin16]; Ins[3].PinMode = GpioPinDriveMode.Input;
            Ins[4] = Pi.Gpio[P1.Pin18]; Ins[4].PinMode = GpioPinDriveMode.Input;
            Ins[5] = Pi.Gpio[P1.Pin22]; Ins[5].PinMode = GpioPinDriveMode.Input;
            Ins[6] = Pi.Gpio[P1.Pin37]; Ins[6].PinMode = GpioPinDriveMode.Input;
            Ins[7] = Pi.Gpio[P1.Pin13]; Ins[7].PinMode = GpioPinDriveMode.Input;
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
            for (int i = 0; i < 8; i++)
                Input[i] = Ins[i].Read();
        }
        #endregion

        #region Out
        public override void Out()
        {

        }
        #endregion

        #endregion
    }
}
