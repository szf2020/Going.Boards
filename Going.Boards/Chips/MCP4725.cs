using Devinno.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Going.Boards.Chips
{
    //internal
    public class MCP4725
    {
        II2CDevice dev;
        byte i2caddr;       

        public MCP4725(byte devid)
        {            
            i2caddr = devid;

            var v = Pi.I2C.Devices.Where(x => x.DeviceId == devid).FirstOrDefault();

            byte[] config = new byte[] { 0x60, 0x00 };
                        
            if (v == null)
            {
                dev = Pi.I2C.AddDevice(devid);
                dev.Write(config);
            }
            else
            {
                dev = Pi.I2C.GetDeviceById(i2caddr);
                dev.Write(config);
            }

        }
        
        public void WriteData(ushort n)
        {
            //byte[] outputData = new byte[] { 0x40, (byte)(value >> 4), (byte)(value << 4) };

            /*
            byte[] data = new byte[3];
            data[0] = 0x40; // Write to DAC register and update output
            data[1] = 0x0B; // Upper 8 bits of output voltage
            data[2] = 0xCD; // Lower 4 bits of output voltage

            dev.Write(data);
            */

            var _powerDownMode = dev.ReadAddressByte(i2caddr);

            byte l = Convert.ToByte(n & 0xFF);
            byte h = Convert.ToByte((n / 256) % 0x0F);
            var n2 = (h | (_powerDownMode << 4)) & 0xFF;

            h = Convert.ToByte(n2);

            dev.Write(new byte[] { i2caddr, h, l });
        }

        /*
        // 출력 전압 값을 계산합니다.
        public int CalculateOutputValue(double voltage)
        {
            int outputValue = (int)(4096 * voltage / 5);
            return outputValue;
        }

        // 출력 전압을 설정합니다.
        public void SetOutputVoltage(double voltage)
        {
            int outputValue = CalculateOutputValue(voltage);
            WriteData(outputValue);
        }
        

        // MCP4725를 슬립 모드로 설정합니다.
        public void SetSleepMode()
        {
            byte[] config = new byte[] { 0x50, 0x00 };
            dev.Write(config);
        }

        // MCP4725를 활성화 상태로 설정합니다.
        public void SetActiveMode()
        {
            byte[] config = new byte[] { 0x60, 0x00 };
            dev.Write(config);
        }
        */

    }
}
