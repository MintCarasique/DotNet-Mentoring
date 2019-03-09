using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSV.Library
{
    public class VisitorEventArgs : EventArgs
    {
        public ProcessAction Action { get; set; }

        public string EntryName { get; set; }

        public VisitorEventArgs(string name, ProcessAction action = ProcessAction.Continue)
        {
            EntryName = name;
            Action = action;
        }
    }
}
