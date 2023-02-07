using System;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Board.Chips
{
    public class PCA9506
    {
        #region Const
        const int PCA9505_BASE_ADDRESS = 0x20;
        #endregion

        #region Member Variable
        II2CDevice dev;

        byte i2caddr;
        #endregion

        #region Constructor
        public PCA9506()
        {
            i2caddr = PCA9505_BASE_ADDRESS;

            Console.WriteLine($"ID : {i2caddr.ToString("X2")}");

            var v = Pi.I2C.Devices.Where(x => x.DeviceId == i2caddr).FirstOrDefault();
            if (v == null)
            { Console.WriteLine("AddDevice"); dev = Pi.I2C.AddDevice(i2caddr); }
            else
            { Console.WriteLine("GetDevice"); dev = Pi.I2C.GetDeviceById(i2caddr); }
        }

        public PCA9506(byte devid)
        {
            i2caddr = devid;

            Console.WriteLine($"ID : {i2caddr.ToString("X2")}");

            var v = Pi.I2C.Devices.Where(x => x.DeviceId == i2caddr).FirstOrDefault();
            if (v == null)
            { Console.WriteLine("AddDevice"); dev = Pi.I2C.AddDevice(i2caddr); }
            else
            { Console.WriteLine("GetDevice"); dev = Pi.I2C.GetDeviceById(i2caddr); }
        }
        #endregion

        #region Method
        public void Setting(byte Port0, byte Port1, byte Port2, byte Port3, byte Port4)
        {
            if (dev != null)
            {
                //var ba = new byte[] { 0x98, 0, 0, 0, 0, 0 };
                //dev.Write(ba);

                dev.WriteAddressByte(0x18, Port0);
                dev.WriteAddressByte(0x19, Port1);
                dev.WriteAddressByte(0x1A, Port2);
                dev.WriteAddressByte(0x1B, Port3);
                dev.WriteAddressByte(0x1C, Port4);
            }
        }

        public void Output(byte Port0, byte Port1, byte Port2, byte Port3, byte Port4)
        {
            if (dev != null)
            {
                //var ba = new byte[] { 0x88, Port0, Port1, Port2, Port3, Port4 };
                //dev.Write(ba);

                dev.WriteAddressByte(0x08, Convert.ToByte(Port0 ^ 0xFF));
                dev.WriteAddressByte(0x09, Convert.ToByte(Port1 ^ 0xFF));
                dev.WriteAddressByte(0x0A, Convert.ToByte(Port2 ^ 0xFF));
                dev.WriteAddressByte(0x0B, Convert.ToByte(Port3 ^ 0xFF));
                dev.WriteAddressByte(0x0C, Convert.ToByte(Port4 ^ 0xFF));
            }
        }

        public byte[] Input()
        {
            byte[] ret = null;
            if (dev != null)
            {
                //var ba = new byte[] { 0x88, Port0, Port1, Port2, Port3, Port4 };
                //dev.Write(ba);

                byte v0, v1, v2, v3, v4;
                v0 = dev.ReadAddressByte(0x00);
                v1 = dev.ReadAddressByte(0x01);
                v2 = dev.ReadAddressByte(0x02);
                v3 = dev.ReadAddressByte(0x03);
                v4 = dev.ReadAddressByte(0x04);

                ret = new byte[] { v0, v1, v2, v3, v4 };
            }
            return ret;
        }
        #endregion
    }

    #region class : IOPort
    public class IOPort
    {
        public byte[] Port { get; set; } = new byte[5];

        public IOPort() { }
        public IOPort(byte v1, byte v2, byte v3, byte v4, byte v5)
        {
            Port[0] = v1;
            Port[1] = v2;
            Port[2] = v3;
            Port[3] = v4;
            Port[4] = v5;
        }
    }
    #endregion

    #region enum : PCA9506Reg
    public enum PCA9506Reg : byte
    {
        AI_ON = 0x80,
        AI_OFF = 0x00,

        BASE_ADDRESS = 0x20,
        A0 = 0x01,
        A1 = 0x02,
        A2 = 0x04,

        IP = 0x00,
        IP0 = 0x00,
        IP1 = 0x01,
        IP2 = 0x02,
        IP3 = 0x03,
        IP4 = 0x04,
        OP = 0x08,
        OP0 = 0x08,
        OP1 = 0x09,
        OP2 = 0x0A,
        OP3 = 0x0B,
        OP4 = 0x0C,
        PI = 0x10,
        PI0 = 0x10,
        PI1 = 0x11,
        PI2 = 0x12,
        PI3 = 0x13,
        PI4 = 0x14,
        IOC = 0x18,
        IOC0 = 0x18,
        IOC1 = 0x19,
        IOC2 = 0x1A,
        IOC3 = 0x1B,
        IOC4 = 0x1C,
        MSK = 0x20,
        MSK0 = 0x20,
        MSK1 = 0x21,
        MSK2 = 0x22,
        MSK3 = 0x23,
        MSK4 = 0x24,
    }
    #endregion
}
