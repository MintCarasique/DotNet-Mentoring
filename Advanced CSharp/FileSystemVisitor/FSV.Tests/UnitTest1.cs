using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FSV.Library;

namespace FileSystemVisitorTests
{
    [TestFixture]
    public class FileSystemVisitorTests
    {
        private string _baseDirectory;
        private FileSystemVisitor _visitor;

        [SetUp]
        public void Initialize()
        {
            _baseDirectory = @"D:\\FakeDirectory\";
            _visitor = new FileSystemVisitor();
        }

        [Test]
        public void VisitDirectory_WhenCalled_StartAndFinishEventsAreTriggered()
        {
            // Arrange.
            bool wasStartCalled = false, wasFinishCalled = false;
            _visitor.Start += (object sender, EventArgs e) => wasStartCalled = true;
            _visitor.Finish += (object sender, EventArgs e) => wasFinishCalled = true;

            // Act.
            _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.IsTrue(wasStartCalled);
            Assert.IsTrue(wasFinishCalled);
        }

        [Test]
        public void VisitDirectory_WhenDirectoryExists_DirectoryFindedAndFilteredDirectoryFindedAreTriggered()
        {
            // Arrange.
            bool wasDirectoryFindedCalled = false, wasFilteredDirectoryFindedCalled = false;
            _visitor.DirectoryFound += (object sender, VisitorEventArgs e) =>
                wasDirectoryFindedCalled = true;
            _visitor.FilteredDirectoryFound += (object sender, VisitorEventArgs e) =>
                wasFilteredDirectoryFindedCalled = true;

            // Act.
            _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.IsTrue(wasDirectoryFindedCalled);
            Assert.IsTrue(wasFilteredDirectoryFindedCalled);
        }

        [Test]
        public void VisitDirectory_WhenFileExists_FileFindedAndFilteredFileFindedAreTriggered()
        {
            // Arrange.
            bool wasFileFindedCalled = false, wasFilteredFileFindedCalled = false;
            _visitor.FileFound += (object sender, VisitorEventArgs e) =>
                wasFileFindedCalled = true;
            _visitor.FilteredFileFound += (object sender, VisitorEventArgs e) =>
                wasFilteredFileFindedCalled = true;
            _baseDirectory += @"FakeDirectoryWithFile\";

            // Act.
            _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.IsTrue(wasFileFindedCalled);
            Assert.IsTrue(wasFilteredFileFindedCalled);
        }

        [Test]
        public void VisitDirectory_WhenFilterIsSet_FilesAndDirectoriesAreFiltered()
        {
            // Arrange.
            Func<string, bool> filter = (string path) => true;
            _visitor = new FileSystemVisitor(filter);

            // Act.
            List<string> result = _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void VisitDirectory_WhenExcludeFlagIsSet_FileOrDirectoryIsExcluded()
        {
            // Arrange.
            _visitor.FileFound += (object sender, VisitorEventArgs e) =>
                e.Action = ProcessAction.Exclude;
            _baseDirectory += @"FakeDirectoryWithFile\";

            // Act.
            List<string> result = _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void VisitDirectory_WhenStopFlagIsSet_SearchStops()
        {
            // Arrange.
            _visitor.FileFound += (object sender, VisitorEventArgs e) =>
                e.Action = ProcessAction.Interrupt;

            // Act.
            List<string> result = _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            // Only 'FakeDirectoryWithFile' was found, then search was stopped.
            Assert.AreEqual(1, result.Count);
        }
    }
}