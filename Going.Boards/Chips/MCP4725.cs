using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Chips
{
    internal class MCP4725
    {
        #region Const
        const int MCP4725_MAXVALUE = 4095;

        //  ERRORS
        const int MCP4725_OK = 0;
        const int MCP4725_VALUE_ERROR = -999;
        const int MCP4725_REG_ERROR = -998;
        const int MCP4725_NOT_CONNECTED = -997;

        //  powerDown Mode - TODO ENUM?
        const int MCP4725_PDMODE_NORMAL = 0x00;
        const int MCP4725_PDMODE_1K = 0x01;
        const int MCP4725_PDMODE_100K = 0x02;
        const int MCP4725_PDMODE_500K = 0x03;

        const int MCP4725_MIDPOINT = 2048;

        const byte MCP4725_DAC = 0x40;
        const byte MCP4725_DACEEPROM = 0x60;
        #endregion

        #region Member Variable
        II2CDevice dev;

        byte i2caddr;
        byte _powerDownMode = 0;
        ushort _lastValue = 0;
        #endregion

        #region Constructor
        internal MCP4725(byte devid)
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
            _powerDownMode = ReadPowerDownMode();
            _lastValue = ReadDAC();
        }
        #endregion

        #region WriteDAC
        public void WriteDAC(ushort value)
        {
            if (value != _lastValue)
            {
                WriteFastMode(value);
                _lastValue = value;
            }
        }
        #endregion

        #region Ready
        bool Ready()
        {
            var ret = false;
            if (dev != null)
            {
                var v = dev.Read();
                ret = (v & 0x80) > 0;
            }
            return ret;
        }
        #endregion
        #region WritePowerDownMode
        void WritePowerDownMode(byte PDM, bool EEPROM)
        {
            _powerDownMode = Convert.ToByte(PDM & 0x03); // mask PDM bits only (written later low level)
            WriteDAC(_lastValue, EEPROM);
        }
        #endregion
        #region ReadPowerDownMode
        byte ReadPowerDownMode()
        {
            byte ret = 0;
            if (dev != null)
            {
                while (!Ready()) { }
                byte v = dev.Read();
                ret = Convert.ToByte((v >> 1) & 0x03);
            }
            return ret;
        }
        #endregion
        #region WriteDAC
        void WriteDAC(ushort value, bool EEPROM)
        {
            value = Convert.ToUInt16(value & 0x0FFF);
            while (!Ready()) { }
            WriteRegisterMode(value, EEPROM ? MCP4725_DACEEPROM : MCP4725_DAC);
        }
        #endregion
        #region ReadDAC
        ushort ReadDAC()
        {
            ushort ret = 0;
            if (dev != null)
            {
                while (!Ready()) { }
                byte[] buffer = dev.Read(3);
                int value = buffer[1];
                value = value << 4;
                value = value + (buffer[2] >> 4);
                ret = Convert.ToUInt16(value & 0xFFFF);
            }
            return ret;
        }
        #endregion
        #region WriteRegisterMode
        void WriteRegisterMode(ushort value, byte reg)
        {
            if (dev != null)
            {
                byte h = Convert.ToByte(value / 16);
                byte l = Convert.ToByte(value & 0x0F << 4);
                reg = Convert.ToByte(reg | (_powerDownMode << 1));

                dev.Write(new byte[] { reg, h, l });
            }
        }
        #endregion
        #region WriteFastMode
        void WriteFastMode(ushort value)
        {
            if (dev != null)
            {
                while (!Ready()) { }

                byte l = Convert.ToByte(value & 0xFF);
                byte h = Convert.ToByte((value / 256) & 0x0F);
                h = Convert.ToByte(h | (_powerDownMode << 4));
                dev.WriteAddressByte(h, l);
            }
        }
        #endregion
        #endregion

    }
}

