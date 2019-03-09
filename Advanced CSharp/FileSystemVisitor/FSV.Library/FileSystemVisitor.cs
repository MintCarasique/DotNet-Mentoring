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
        private readonly Func<string, bool> _filter;

        public event EventHandler<EventArgs> Start, Finish;

        public event EventHandler<VisitorEventArgs> FileFound, DirectoryFound, FilteredFileFound, FilteredDirectoryFound;

        public FileSystemVisitor(Func<string, bool> filter = null)
        {
            if (filter != null)
            {
                _filter = filter;
            }
            else
            {
                _filter = (string path) => false;
            }
        }

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
                    action = EntryProcess(entry, fileName, DirectoryFound, FilteredDirectoryFound);
                }
                else
                {
                    action = EntryProcess(entry, fileName, FileFound, FilteredFileFound);
                }
                yield return entry;
            }
        }

        private ProcessAction EntryProcess(
            string entry, 
            string entryName, 
            EventHandler<VisitorEventArgs> foundHandler,
            EventHandler<VisitorEventArgs> filterHandler)
        {
            VisitorEventArgs e = new VisitorEventArgs(entryName);

            OnEvent(foundHandler, e);

            if (!_filter(entry))
            {
                this.OnEvent(filterHandler, e);
                return e.Action;
            }

            return e.Action;
        }

        protected virtual void OnEvent<T>(EventHandler<T> eventHandler, T eventArgs)
        {
            eventHandler?.Invoke(this, eventArgs);
        }
    }
}
