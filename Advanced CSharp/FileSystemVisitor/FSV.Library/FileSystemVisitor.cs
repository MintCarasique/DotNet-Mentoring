using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FSV.Library
{
    public class FileSystemVisitor
    {
        private readonly IFileSystem _fileSystem;

        private readonly Func<string, bool> _filter;

        public event EventHandler<EventArgs> Start, Finish;

        public event EventHandler<VisitorEventArgs> FileFound, DirectoryFound, FilteredFileFound, FilteredDirectoryFound;


        public FileSystemVisitor(Func<string, bool> filter = null)
        {
            _fileSystem = new FileSystem();
            if (filter != null)
            {
                _filter = filter;
            }
            else
            {
                _filter = (string path) => false;
            }
        }

        internal FileSystemVisitor(IFileSystem fileSystem, Func<string, bool> filter = null)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<string> PerformProcess(string startPath)
        {
            OnEvent(Start, new EventArgs());

            string[] entries = _fileSystem.Directory.EnumerateFileSystemEntries(startPath, "*", SearchOption.AllDirectories).ToArray();

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

                if (_fileSystem.Directory.Exists(entry))
                {
                    action = EntryProcess(entry, fileName, DirectoryFound, FilteredDirectoryFound);
                }
                else
                {
                    action = EntryProcess(entry, fileName, FileFound, FilteredFileFound);
                }

                if (action == ProcessAction.Interrupt)
                {
                    yield break;
                }
                else if (action == ProcessAction.Continue)
                {
                    yield return entry;
                }
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

            if (e.Action != ProcessAction.Continue)
            {
                return e.Action;
            }

            if (!_filter(entry))
            {
                this.OnEvent(filterHandler, e);
                return e.Action;
            }

            return ProcessAction.Exclude;
        }

        protected virtual void OnEvent<T>(EventHandler<T> eventHandler, T eventArgs)
        {
            eventHandler?.Invoke(this, eventArgs);
        }
    }
}
