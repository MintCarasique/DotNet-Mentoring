using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSV.Library
{
    public class FileSystemVisitor
    {
        public event EventHandler<EventArgs> Start, Finish;

        public IEnumerable<string> PerformProcess(string startPath)
        {
            OnEvent(Start, new EventArgs());

            string[] entries = Directory.GetFileSystemEntries(startPath, "*", SearchOption.AllDirectories);            

            OnEvent(Finish, new EventArgs());

            return entries;
        }

        protected virtual void OnEvent<T>(EventHandler<T> eventHandler, T eventArgs)
        {
            eventHandler?.Invoke(this, eventArgs);
        }
    }
}
