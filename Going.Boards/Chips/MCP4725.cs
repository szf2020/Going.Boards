using System;
using System.Linq;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Going.Boards.Chips
{
    //internal
    internal class MCP4725
    {
        II2CDevice dev;
        
        public MCP4725(byte devid)
        {
            byte i2caddr = 0x60;
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

        public void WriteData(GoingPLC engine, ushort n)
        {
            var v = n & 0x0fff;
            //dev.WriteAddressByte(0, 0x40);
            //dev.WriteAddressByte((v & 0xff00) >> 8, Convert.ToByte(v & 0xff));
            //System.Threading.Thread.Sleep(5);
            //Console.WriteLine($"D:{n}");
            engine.I2C.Add(new I2cWork(dev, new byte[] { 0, 0x40, Convert.ToByte((v & 0xff00) >> 8), Convert.ToByte(v & 0xff) }));
        }        
    }
}
