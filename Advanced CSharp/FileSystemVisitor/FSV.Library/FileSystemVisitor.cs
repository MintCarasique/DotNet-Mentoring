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

        public event EventHandler<VisitorEventArgs> FileFound, DirectoryFound, FilteredFileFound, FilteredDirectoryFound;

        public IEnumerable<string> PerformProcess(string startPath)
        {
            OnEvent(Start, new EventArgs());

            string[] entries = Directory.GetFileSystemEntries(startPath, "*", SearchOption.AllDirectories);

            foreach (string entry in this.EntriesProcessor(entries))
            {
                yield return entry;
            }

            OnEvent(Finish, new EventArgs());
        }

        private IEnumerable<string> EntriesProcessor(string[] entries)
        {
            foreach(var entry in entries)
            {
                var fileName = Path.GetFileName(entry);
                ProcessAction action;

                if (Directory.Exists(entry))
                {
                    action = EntryProcess(entry, fileName, DirectoryFound);
                }
                else
                {
                    action = EntryProcess(entry, fileName, FileFound);
                }
                yield return entry;
            }
        }

        private ProcessAction EntryProcess(
            string entry, 
            string entryName, 
            EventHandler<VisitorEventArgs> foundHandler)
        {
            VisitorEventArgs e = new VisitorEventArgs(entryName);

            OnEvent(foundHandler, e);

            return e.Action;
        }

        protected virtual void OnEvent<T>(EventHandler<T> eventHandler, T eventArgs)
        {
            eventHandler?.Invoke(this, eventArgs);
        }
    }
}
