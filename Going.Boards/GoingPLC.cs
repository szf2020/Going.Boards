using Devinno.Communications.TextComm.TCP;
using Devinno.Data;
using Devinno.PLC.Ladder;
using Going.Boards.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Going.Boards
{
    public class GoingPLC : LadderEngine
    {
        #region Properties 
        public List<IGoingBoard> Boards { get; } = new List<IGoingBoard>();
        public I2cScheduler I2C { get; } = new I2cScheduler();
        #endregion

        #region Member Variable
        TextCommTCPSlave comm;
        
       #endregion

        public GoingPLC()
        {
            Pi.Init<BootstrapWiringPi>();

            #region Communication
            comm = new TextCommTCPSlave { DisconnectCheckTime = 3000 };
            comm.Start();
            comm.MessageRequest += (o, s) => {

                if (s.Command == 10)
                {
                    try
                    {
                        var v = Serialize.JsonDeserialize<List<Shield>>(s.RequestMessage);
                        if (v != null && v.Count > 0)
                        {

                        }
                    }
                    catch { }
                }

            };
            #endregion
           
            I2C.Start();
        }

        #region Override
        #region OnEngineStart
        public override void OnEngineStart()
        {
            foreach (var v in Boards) v.Begin(this);
            I2C.Start();

            base.OnEngineStart();
        }
        #endregion
        #region OnEngineStop
        public override void OnEngineStop()
        {
            I2C.Stop();

            base.OnEngineStop();
        }
        #endregion

        #region OnDeviceLoad
        public override void OnDeviceLoad()
        {
            //try
            //{
                foreach (var v in Boards)
                {
                    v.Load(this);
                    v.InputMapping(this);
                }
            //}
            //catch (Exception e) { File.AppendAllText("/home/pi/load.txt", DateTime.Now + " : " + e.Message + "\r\n" + e.StackTrace); }

            base.OnDeviceLoad();
        }
        #endregion
        #region OnDeviceOutput
        public override void OnDeviceOuput()
        {
            //try
            //{
                foreach (var v in Boards)
                {
                    v.OutputMapping(this);
                    v.Out(this);
                }
            //}
            //catch (Exception e) { File.AppendAllText("/home/pi/out.txt", DateTime.Now + " : " + e.Message + "\r\n" + e.StackTrace); }

            base.OnDeviceOuput();
        }
        #endregion
        #endregion


    }

    class Shield
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    #region class : I2cWork
    public class I2cWork
    {
        public II2CDevice Device { get; private set; }
        public byte[] Data { get; private set; }

        public I2cWork(II2CDevice Device, byte[] Data)
        {
            this.Device = Device;
            this.Data = Data;
        }
    }
    #endregion
    #region class : I2cScheduler
    public class I2cScheduler
    {
        #region Properties
        public int Interval { get; set; } = 10;
        #endregion

        #region Member Variable
        Queue<I2cWork> lsI2C = new Queue<I2cWork>();
        Thread th;
        bool bIsStart = false;
        #endregion

        #region Constructor
        public I2cScheduler()
        {

        }
        #endregion

        #region Method
        #region Add
        public void Add(I2cWork w) => lsI2C.Enqueue(w);
        #endregion
        #region Start
        public void Start()
        {
            if (!bIsStart)
            {
                th = new Thread(new ThreadStart(run)) { IsBackground = true };
                th.Start();
            }
        }
        #endregion
        #region Stop
        public void Stop()
        {
            bIsStart = false;
        }
        #endregion
        #region run : Thread
        void run()
        {
            bIsStart = true;
            while(bIsStart)
            {
                if(lsI2C.Count > 0)
                {
                    var v = lsI2C.Dequeue();
                    if (v.Device != null && v.Data != null)
                    {
                        
                        for (int i = 0; i < v.Data.Length; i++)
                        {
                            Console.Write(v.Data[i].ToString("X2") + " ");
                        }
                        Console.WriteLine("");

                        //v.Device.Write(v.Data);

                        if (v.Data.Length % 2 == 0)
                        {
                            for (int i = 0; i < v.Data.Length; i += 2)
                            {
                                v.Device.WriteAddressByte(v.Data[i], v.Data[i + 1]);                                
                            }
                            
                        }
                        
                    }
                }
                Thread.Sleep(Interval);
            }
        }
        #endregion
        #endregion
    }
    #endregion
}