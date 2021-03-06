using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FSV.Library;
using NUnit.Framework;

namespace FSV.Tests
{
    [TestFixture]
    public class FileSystemVisitorTests
    {
        private string _baseDirectory;
        private FileSystemVisitor _visitor;
        private MockFileSystem _mockFileSystem;

        [SetUp]
        public void Initialize()
        {
            _mockFileSystem = new MockFileSystem();
            _mockFileSystem.AddDirectory(@"D:\FakeDirectory\");
            _mockFileSystem.AddDirectory(@"D:\FakeDirectory\FakeDirectoryWithFile");
            _mockFileSystem.AddDirectory(@"D:\FakeDirectory\FakeDirectoryWithoutFile");
            _mockFileSystem.AddFile(@"D:\FakeDirectory\FakeDirectoryWithFile\file.txt", MockFileData.NullObject);

            _baseDirectory = @"D:\\FakeDirectory\";

            _visitor = new FileSystemVisitor(_mockFileSystem);
        }

        [Test]
        public void VisitDirectory_WhenCalled_StartAndFinishEventsAreTriggered()
        {
            // Arrange.
            bool wasStartCalled = false, wasFinishCalled = false;
            _visitor.Start += (sender, e) => wasStartCalled = true;
            _visitor.Finish += (sender, e) => wasFinishCalled = true;

            // Act.
            var list = _visitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.IsTrue(wasStartCalled);
            Assert.IsTrue(wasFinishCalled);
        }

        [Test]
        public void VisitDirectory_WhenDirectoryExists_DirectoryFindedAndFilteredDirectoryFindedAreTriggered()
        {
            // Arrange.
            bool wasDirectoryFindedCalled = false, wasFilteredDirectoryFindedCalled = false;
            _visitor.DirectoryFound += (sender, e) =>
                wasDirectoryFindedCalled = true;
            _visitor.FilteredDirectoryFound += (sender, e) =>
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
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void VisitDirectory_WhenNullStartPathPassed_ThrowArgumentNullException()
        {
            // Arrange.
            bool wasStartCalled = false, wasFinishCalled = false;
            _visitor.Start += (object sender, EventArgs e) => wasStartCalled = true;
            _visitor.Finish += (object sender, EventArgs e) => wasFinishCalled = true;

            // Act.
            void FsvPerform() => _visitor.PerformProcess(null).ToList();

            // Assert.
            Assert.Throws<ArgumentNullException>(FsvPerform);
        }

        [Test]
        public void VisitDirectory_WhenNullFileSystemPassed_ThrowNullReferenceException()
        {
            // Arrange.
            var nullVisitor = new FileSystemVisitor(null, null);

            // Act.
            void FsvPerform() => nullVisitor.PerformProcess(_baseDirectory).ToList();

            // Assert.
            Assert.Throws<NullReferenceException>(FsvPerform);
        }
    }
}