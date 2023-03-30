using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Going.Boards
{
    public abstract class GoingBoard
    {
        public GoingPLC PLC { get; internal set; }
        public HardwareList Hardwares { get; protected set; }

        public abstract void Begin();
        public abstract void End();
        public abstract void Out();
        public abstract void Load();
        public abstract void Update();
    }

    #region enum : HardwareType
    public enum HardwareType { INPUT, OUTPUT, DAC, ADC }
    #endregion
    #region interface : IHardware
    public interface IHardware
    {
        public string Name { get; }
        public string Address { get; set; }
        public HardwareType Type { get; }
    }
    #endregion
    #region class : HardwareInput
    public class HardwareInput : IHardware
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public HardwareType Type => HardwareType.INPUT;
        public bool Value { get; set; }

        public HardwareInput(string Name) => this.Name = Name;
    }
    #endregion
    #region class : HardwareOutput
    public class HardwareOutput : IHardware
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public HardwareType Type => HardwareType.OUTPUT;
        public bool Value { get; set; }

        public HardwareOutput(string Name) => this.Name = Name;
    }
    #endregion
    #region class : HardwareDAC
    public class HardwareDAC : IHardware
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public HardwareType Type => HardwareType.DAC;
        public int Value { get; set; }

        public HardwareDAC(string Name) => this.Name = Name;
    }
    #endregion
    #region class : HardwareADC
    public class HardwareADC : IHardware
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public HardwareType Type => HardwareType.ADC;
        public int Value { get; set; }

        public HardwareADC(string Name) => this.Name = Name;
    }
    #endregion
    #region class : HardwareList
    public class HardwareList
    {
        #region Indexer
        public IHardware this[int index] { get => vs[index]; }
        public IHardware this[string name]
        {
            get
            {
                IHardware ret = null;
                if (vs != null)
                {
                    var dic = vs.ToDictionary(x => x.Name);
                    ret = dic[name];
                }
                return ret;
            }
        }
        #endregion

        #region Properties
        public int Count => (vs != null ? vs.Length : 0);

        public IEnumerable<string> Keys => (vs != null ? vs.Select(x => x.Name) : null);
        #endregion

        #region Member Variable
        IHardware[] vs = null;
        #endregion

        #region Constructor
        public HardwareList(IHardware[] vs)
        {
            this.vs = vs;
        }
        #endregion

        #region Method
        #region ContainsName
        public bool ContainsName(string name)
        {
            bool ret = false;
            if(vs != null)
            {
                var dic = vs.ToDictionary(x => x.Name);
                ret = dic.ContainsKey(name);
            }
            return ret;
        }
        #endregion
        #region Contains
        public bool Contains(IHardware hardware)
        {
            bool ret = false;
            if(vs != null)
            {
                ret = vs.Contains(hardware);
            }
            return ret;
        }
        #endregion
        #region ToArray
        public IHardware[] ToArray() => vs;
        #endregion
        #endregion
    }
    #endregion
}
