using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Chips
{
    internal class MCP23017
    {
        #region Member Variable
        II2CDevice dev;

        byte i2caddr;
        #endregion

        #region Constructor
        public MCP23017(byte devid)
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
            WriteReg(Register.IOCON, 0b00100000);
            WriteReg(Register.GPPU_A, 0xFF, 0xFF);
        }
        #endregion
        
        #region PortMode
        public void PortMode(Port port, PinMode mode) => PortMode(port, mode, 0xFF, 0x00);
        public void PortMode(Port port, PinMode mode, byte pullups, byte inverted)
        {
            if (dev != null)
            {
                switch (port)
                {
                    case Port.A:
                        WriteReg(Register.IODIR_A, (byte)mode);
                        WriteReg(Register.GPPU_A, pullups);
                        WriteReg(Register.IPOL_A, inverted);
                        break;
                    case Port.B:
                        WriteReg(Register.IODIR_B, (byte)mode);
                        WriteReg(Register.GPPU_B, pullups);
                        WriteReg(Register.IPOL_B, inverted);
                        break;
                }
            }
        }
        #endregion
        #region WritePort
        public void WritePort(Port port, byte value)
        {
            if (dev != null)
            {
                switch (port)
                {
                    case Port.A: WriteReg(Register.GPIO_A, value); break;
                    case Port.B: WriteReg(Register.GPIO_B, value); break;
                }
            }
        }
        #endregion
        #region ReadPort
        public byte ReadPort(Port port)
        {
            byte ret = 0;
            if (dev != null)
            {
                switch (port)
                {
                    case Port.A: ret = ReadReg(Register.GPIO_A); break;
                    case Port.B: ret = ReadReg(Register.GPIO_B); break;
                }
            }
            return ret;
        }
        #endregion

        #region WriteReg
        void WriteReg(Register reg, byte value)
        {
            dev.WriteAddressByte((byte)reg, value); //(byte)reg
        }

        void WriteReg(Register reg, byte portA, byte portB)
        {
            dev.WriteAddressWord((byte)reg, Convert.ToUInt16((portA << 8) | portB));
            
        }
        #endregion
        #region ReadReg
        byte ReadReg(Register reg)
        {
            dev.Write((byte)reg);
            return dev.Read();
        }

        void ReadReg(Register reg, ref byte portA, ref byte portB)
        {
            dev.Write((byte)reg);
            byte[] vals = dev.Read(2);

            if (vals.Length == 2)
            {
                portA = vals[0];
                portB = vals[1];
            }
        }
        #endregion
        #endregion

        #region Enums
        #region enum : Register
        public enum Register : byte
        {
            IODIR_A = 0x00,         ///< Controls the direction of the data I/O for port A.
            IODIR_B = 0x01,         ///< Controls the direction of the data I/O for port B.
            IPOL_A = 0x02,          ///< Configures the polarity on the corresponding GPIO_ port bits for port A.
            IPOL_B = 0x03,          ///< Configures the polarity on the corresponding GPIO_ port bits for port B.
            GPINTEN_A = 0x04,       ///< Controls the interrupt-on-change for each pin of port A.
            GPINTEN_B = 0x05,       ///< Controls the interrupt-on-change for each pin of port B.
            DEFVAL_A = 0x06,        ///< Controls the default comparaison value for interrupt-on-change for port A.
            DEFVAL_B = 0x07,        ///< Controls the default comparaison value for interrupt-on-change for port B.
            INTCON_A = 0x08,        ///< Controls how the associated pin value is compared for the interrupt-on-change for port A.
            INTCON_B = 0x09,        ///< Controls how the associated pin value is compared for the interrupt-on-change for port B.
            IOCON = 0x0A,           ///< Controls the device.
            GPPU_A = 0x0C,          ///< Controls the pull-up resistors for the port A pins.
            GPPU_B = 0x0D,          ///< Controls the pull-up resistors for the port B pins.
            INTF_A = 0x0E,          ///< Reflects the interrupt condition on the port A pins.
            INTF_B = 0x0F,          ///< Reflects the interrupt condition on the port B pins.
            INTCAP_A = 0x10,        ///< Captures the port A value at the time the interrupt occured.
            INTCAP_B = 0x11,        ///< Captures the port B value at the time the interrupt occured.
            GPIO_A = 0x12,          ///< Reflects the value on the port A.
            GPIO_B = 0x13,          ///< Reflects the value on the port B.
            OLAT_A = 0x14,          ///< Provides access to the port A output latches.
            OLAT_B = 0x15,          ///< Provides access to the port B output latches.
        }
        #endregion
        #region enum : Port 
        public enum Port : byte
        {
            A = 0x00, 
            B = 0x01
        }
        #endregion
        #region enum : PinMode
        public enum PinMode : byte
        {
            INPUT = 0x01,
            OUTPUT = 0x00,
        }
        #endregion
        #endregion
    }
}
