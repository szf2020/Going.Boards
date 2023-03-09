using Going.Boards.Interfaces;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards
{
    public class SD8R : GoingBoard
    {
        #region Properties
        public override bool[] Input { get; } = new bool[8];
        public override bool[] Output { get; } = new bool[8];
        public override ushort[] DAOUT { get; } = new ushort[1];
        #endregion

        #region Member Variable
        IGpioPin[] Outs = new IGpioPin[8];
        IGpioPin[] Ins = new IGpioPin[8];
        #endregion

        #region Constructor
        public SD8R()
        {
            Outs[0] = Pi.Gpio[P1.Pin29]; Outs[0].PinMode = GpioPinDriveMode.Output;
            Outs[1] = Pi.Gpio[P1.Pin31]; Outs[1].PinMode = GpioPinDriveMode.Output;
            Outs[2] = Pi.Gpio[P1.Pin32]; Outs[2].PinMode = GpioPinDriveMode.Output;
            Outs[3] = Pi.Gpio[P1.Pin33]; Outs[3].PinMode = GpioPinDriveMode.Output;
            Outs[4] = Pi.Gpio[P1.Pin36]; Outs[4].PinMode = GpioPinDriveMode.Output;
            Outs[5] = Pi.Gpio[P1.Pin11]; Outs[5].PinMode = GpioPinDriveMode.Output;
            Outs[6] = Pi.Gpio[P1.Pin12]; Outs[6].PinMode = GpioPinDriveMode.Output;
            Outs[7] = Pi.Gpio[P1.Pin35]; Outs[7].PinMode = GpioPinDriveMode.Output;
        }
        #endregion

        #region Method
        #region Begin
        public override void Begin(GoingPLC engine)
        {
            Load(engine);
            Out(engine);
        }
        #endregion

        #region Load
        public override void Load(GoingPLC engine)
        {

        }
        #endregion

        #region Out
        public override void Out(GoingPLC engine)
        {
            for (int i = 0; i < 8; i++)
                Outs[i].Write(Output[i]);
        }
        #endregion

        #endregion
    }
}
