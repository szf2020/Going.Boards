using Devinno.Extensions;
using System;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Board.Chips
{
    public class MCP23017
    {

        #region Member Variable
        II2CDevice dev;

        byte i2caddr;
        #endregion

        #region Constructor
        public MCP23017(byte devid)
        {
            i2caddr = devid;

            var v = Pi.I2C.Devices.Where(x => x.DeviceId == devid).FirstOrDefault();

            if (v == null)
            {
                dev = Pi.I2C.AddDevice(devid);
            }
            else
            {
                dev = Pi.I2C.GetDeviceById(i2caddr);
            }

        }
        #endregion

        #region Method
        #region init
        public void init()
        {
            writeRegister(MCP23017Register.IOCON, 0b00100000);
            writeRegister(MCP23017Register.GPPU_A, 0xFF, 0xFF);
        }
        #endregion
        #region portMode
        public void portMode(MCP23017Port port, Direction directions, byte pullups = 0xFF, byte inverted = 0x00)
        {
            writeRegister(add(MCP23017Register.IODIR_A, port), (byte)directions);
            writeRegister(add(MCP23017Register.GPPU_A, port), pullups);
            writeRegister(add(MCP23017Register.IPOL_A, port), inverted);
        }
        #endregion
        #region pinMode
        void pinMode(byte pin, Direction mode, bool inverted = false)
        {
            MCP23017Register iodirreg = MCP23017Register.IODIR_A;
            MCP23017Register pullupreg = MCP23017Register.GPPU_A;
            MCP23017Register polreg = MCP23017Register.IPOL_A;
            byte iodir, pol, pull;

            if (pin > 7)
            {
                iodirreg = MCP23017Register.IODIR_B;
                pullupreg = MCP23017Register.GPPU_B;
                polreg = MCP23017Register.IPOL_B;
                pin -= 8;
            }

            iodir = readRegister(iodirreg);
            iodir.Bit(pin, mode == Direction.INPUT || mode == Direction.INPUT_PULLUP);

            pull = readRegister(pullupreg);
            pull.Bit(pin, mode == Direction.INPUT_PULLUP);

            pol = readRegister(polreg);
            pol.Bit(pin, inverted);

            writeRegister(iodirreg, iodir);
            writeRegister(pullupreg, pull);
            writeRegister(polreg, pol);
        }
        #endregion
        #region digitalWrite
        public void digitalWrite(byte pin, byte state)
        {
            MCP23017Register gpioreg = MCP23017Register.GPIO_A;
            byte gpio;
            if (pin > 7)
            {
                gpioreg = MCP23017Register.GPIO_B;
                pin -= 8;
            }

            gpio = readRegister(gpioreg);
            gpio.Bit(pin, state == 1);

            writeRegister(gpioreg, gpio);
        }
        #endregion
        #region digitalRead
        public byte digitalRead(byte pin)
        {
            MCP23017Register gpioreg = MCP23017Register.GPIO_A;
            byte gpio;
            if (pin > 7)
            {
                gpioreg = MCP23017Register.GPIO_B;
                pin -= 8;
            }

            gpio = readRegister(gpioreg);
            return (byte)(gpio.Bit(pin) ? 1 : 0);
        }
        #endregion
        #region writePort
        public void writePort(MCP23017Port port, byte value)
        {
            writeRegister(add(MCP23017Register.GPIO_A, port), value);
        }
        #endregion
        #region write
        public void write(ushort value)
        {
            writeRegister(MCP23017Register.GPIO_A, value.Byte0(), value.Byte1());
        }
        #endregion
        #region readPort
        public byte readPort(MCP23017Port port)
        {
            return readRegister(add(MCP23017Register.GPIO_A, port));
        }
        #endregion
        #region read
        public ushort read()
        {
            byte a = readPort(MCP23017Port.A);
            byte b = readPort(MCP23017Port.B);

            return Convert.ToByte(a | b << 8);
        }
        #endregion

        #region writeRegister
        void writeRegister(MCP23017Register reg, byte value)
        {
            dev.Write((byte)reg);
            dev.Write(value);
        }
        #endregion
        #region writeRegister
        void writeRegister(MCP23017Register reg, byte portA, byte portB)
        {
            dev.Write((byte)reg);
            dev.Write(portA);
            dev.Write(portB);
        }
        #endregion
        #region readRegister
        byte readRegister(MCP23017Register reg)
        {
            dev.Write((byte)reg);
            return dev.Read();
        }
        #endregion
        #region readRegister
        void readRegister(MCP23017Register reg, ref byte portA, ref byte portB)
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

        #region add
        MCP23017Register add(MCP23017Register a, MCP23017Port b)
        {
            byte sa = (byte)a;
            byte sb = (byte)b;

            return (MCP23017Register)(sa + sb);
        }
        #endregion
        #endregion
    }

    #region enum Port / InterrptMode / Register
    public enum MCP23017Port : byte { A = 0, B = 1 }
    public enum MCP23017InterrptMode : byte { Seperated = 0, Or = 0b01000000 }
    public enum MCP23017Register : byte
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

    public enum Direction : byte
    {
        INPUT = 0x0,
        OUTPUT = 0x1,
        INPUT_PULLUP = 0x2
    }
    #endregion
}
