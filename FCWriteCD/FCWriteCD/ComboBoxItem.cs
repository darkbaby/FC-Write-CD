using IMAPI2.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWriteCD
{
    class ComboBoxItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public IDiscRecorder2 Disc { get; set; }

        public override string ToString()
        {
            return Key;
        }
    }
}
