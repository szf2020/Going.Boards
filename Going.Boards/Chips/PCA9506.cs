using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Chips
{
    internal class PCA9506
    {
        #region Member Variable
        II2CDevice dev;

        byte i2caddr;
        #endregion

        #region Constructor
        public PCA9506(byte devid)
        {
            i2caddr = devid;
            var v = Pi.I2C.Devices.Where(x => x.DeviceId == i2caddr).FirstOrDefault();

            if (v == null) dev = Pi.I2C.AddDevice(i2caddr);
            else dev = Pi.I2C.GetDeviceById(i2caddr);
        }
        #endregion

        #region Method
        #region Setup
        public void Setup()
        {
            if (dev != null)
            {
                //Interrupt
                WriteReg(Register.MSK, Port.Port0, 0xFF);
                WriteReg(Register.MSK, Port.Port1, 0xFF);
                WriteReg(Register.MSK, Port.Port2, 0xFF);
                WriteReg(Register.MSK, Port.Port3, 0xFF);
                WriteReg(Register.MSK, Port.Port4, 0xFF);

                //Polarity
                WriteReg(Register.PI, Port.Port0, 0);
                WriteReg(Register.PI, Port.Port1, 0);
                WriteReg(Register.PI, Port.Port2, 0);
                WriteReg(Register.PI, Port.Port3, 0);
                WriteReg(Register.PI, Port.Port4, 0);
                
                //PortMode
                PortMode(Port.Port0, PinMode.INPUT);
                PortMode(Port.Port1, PinMode.INPUT);
                PortMode(Port.Port2, PinMode.INPUT);
                PortMode(Port.Port3, PinMode.INPUT);
                PortMode(Port.Port4, PinMode.INPUT);
            }
        }
        #endregion

        #region PortMode
        public void PortMode(Port port, PinMode mode)
        {
            var v = (byte)(mode == PinMode.OUTPUT ? 0x00 : 0xFF);
            if (dev != null) WriteReg(Register.IOC, port, v);
        }
        #endregion
        #region WritePort
        public void WritePort(Port port, byte data)
        {
            if (dev != null) WriteReg(Register.OP, port, Convert.ToByte(data ^ 0xFF));
        }
        #endregion
        #region ReadPort
        public byte ReadPort(Port port)
        {
            if (dev != null) return ReadReg(Register.IP, port);
            else return 0;
        }
        #endregion

        #region WriteReg
        void WriteReg(Register reg, Port port, byte data)
        {
            var vreg = ((byte)reg + ((byte)port % 5)) | (byte)Register.AI_OFF;
            dev.WriteAddressByte(vreg, data);
        }
        #endregion
        #region ReadReg
        byte ReadReg(Register reg, Port port)
        {
            var vreg = ((byte)reg + ((byte)port % 5)) | (byte)Register.AI_OFF;
            return dev.ReadAddressByte(vreg);
        }
        #endregion
        #endregion

        #region Enums
        #region enum : Register
        public enum Register : byte
        {
            AI_ON = 0x80,
            AI_OFF = 0x00,

            BASE_ADDRESS = 0x20,

            A0 = 0x01,
            A1 = 0x02,
            A2 = 0x04,

            IP = 0x00,
            OP = 0x08,
            PI = 0x10,
            IOC = 0x18,
            MSK = 0x20,
        }
        #endregion
        #region enum : Port 
        public enum Port : byte
        {
            Port0 = 0,
            Port1 = 1,
            Port2 = 2,
            Port3 = 3,
            Port4 = 4,
        }
        #endregion
        #region enum : PinMode
        public enum PinMode : byte
        {
            INPUT = 0x0,
            OUTPUT = 0x1,
        }
        #endregion
        #endregion
    }
}
