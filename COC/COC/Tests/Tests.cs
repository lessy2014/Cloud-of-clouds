using System;
using System.Collections.Generic;
using System.IO;
using COC.Application;
using COC.ConsoleInterface;
using COC.Infrastructure;
using NUnit.Framework;
using NUnit.Framework.Internal;
using File = COC.Infrastructure.File;

namespace COC.Tests
{
    [TestFixture]
    public class FinderTests
    {
        private Folder root;
        [SetUp]
        public void SetUp()
        {
            Program.container = App.ConfigureContainer();
            var account = new Account("Account1");
            root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            
            root.Content.Add("Account1", new Folder("Root/Account1", new Dictionary<string, IFileSystemUnit>(), account));
            ((Folder)root.Content["Account1"]).Content.Add("dropbox", new Folder("Root/Account1/dropbox", new Dictionary<string, IFileSystemUnit>(), account));
            ((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content.Add("Folder1", new Folder("Root/Account1/dropbox/Folder1", new Dictionary<string, IFileSystemUnit>(), account));
            ((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content.Add("File1.txt", new File("Root/Account1/dropbox/File1.txt", account));
            ((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content.Add("File1.txt.jpg", new File("Root/Account1/dropbox/File12.jpg", account));
            ((Folder)((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content["Folder1"]).Content.Add("Folder1", new File("Root/Account1/dropbox/Folder1/Folder1", account));
        }

        [Test]
        public void TestFindByExtensionReturnCorrectResult()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>()
                {
                    new ExtensionFilter("txt")
                };
                Finder.Find(root, "", filters, false);
                Assert.AreEqual("Root/Account1/dropbox/File1.txt\r\n", sw.ToString());
            }
        }
        [Test]
        public void TestFindByExtensionReturnEmptyStringWhenNoMatches()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>()
                {
                    new ExtensionFilter("tt")
                };
                Finder.Find(root, "", filters, false);
                Assert.AreEqual("", sw.ToString());
            }
        }
        
        [Test]
        public void TestFindByTypeReturnReturnOnlyCorrectType()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>()
                {
                    new TypeFilter("file")
                };
                Finder.Find(root, "Folder1", filters, false);
                Assert.AreEqual("Root/Account1/dropbox/Folder1/Folder1\r\n", sw.ToString());
            }
        }

        [Test]
        public void TestFindWithoutFiltersCorrect()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>() {};
                Finder.Find(root, "Folder1", filters, false);
                Assert.AreEqual("Root/Account1/dropbox/Folder1\r\nRoot/Account1/dropbox/Folder1/Folder1\r\n", sw.ToString());
            }
        }
        [Test]
        public void TestFindOnlyAccurateResultWhenFullMatchTrue()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>() {};
                Finder.Find(root, "File1.txt", filters, true);
                Assert.AreEqual("Root/Account1/dropbox/File1.txt\r\n", sw.ToString());
            }
        }
        [Test]
        public void TestFindResultWhenFullMatchFalse()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var filters = new List<IFilter>() {};
                Finder.Find(root, "File1", filters, false);
                Assert.AreEqual("Root/Account1/dropbox/File1.txt\r\nRoot/Account1/dropbox/File12.jpg\r\n", sw.ToString());
            }
        }
    }

    [TestFixture]
    public class FileSystemTest
    {
        private Folder root;

        [SetUp]
        public void SetUp()
        {
            Program.container = App.ConfigureContainer();
            var account = new Account("Account1");
            root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            Folder.Root = root;
            root.Content.Add("Account1",
                new Folder("Root/Account1", new Dictionary<string, IFileSystemUnit>(), account)
                {
                    ParentFolder = root
                });
            ((Folder) root.Content["Account1"]).Content.Add("dropbox",
                new Folder("Root/Account1/dropbox", new Dictionary<string, IFileSystemUnit>(), account));
            ((Folder) ((Folder) root.Content["Account1"]).Content["dropbox"]).Content.Add("Folder1",
                new Folder("Root/Account1/dropbox/Folder1", new Dictionary<string, IFileSystemUnit>(), account));
            ((Folder) ((Folder) root.Content["Account1"]).Content["dropbox"]).Content.Add("File1.txt",
                new File("Root/Account1/dropbox/File1.txt", account));
            ((Folder) ((Folder) root.Content["Account1"]).Content["dropbox"]).Content.Add("File1.txt.jpg",
                new File("Root/Account1/dropbox/File12.jpg", account));
            ((Folder) ((Folder) ((Folder) root.Content["Account1"]).Content["dropbox"]).Content["Folder1"]).Content.Add(
                "Folder1", new File("Root/Account1/dropbox/Folder1/Folder1", account));
            FileSystemManager.CurrentFolder = (Folder)((Folder)root.Content["Account1"]).Content["dropbox"];
        }

        [Test]
        public void TestDirReturnCorrectInfo()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                OutputManager.WriteFolderData(FileSystemManager.CurrentFolder);
                Assert.AreEqual("Folder1\r\nFile1.txt\r\nFile1.txt.jpg\r\n", sw.ToString());
            }
        }
        
        [Test]
        public void TestCdChangeCurrentFolder()
        {
            FileSystemManager.MoveToFolder("Folder1");
            Assert.AreEqual((Folder)((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content["Folder1"], FileSystemManager.CurrentFolder);
        }
        
        [Test]
        public void TestCdWorkThrowFewFolders()
        {
            FileSystemManager.CurrentFolder = (Folder) root.Content["Account1"];
            FileSystemManager.MoveToFolder("dropbox/Folder1");
            Assert.AreEqual((Folder)((Folder)((Folder)root.Content["Account1"]).Content["dropbox"]).Content["Folder1"], FileSystemManager.CurrentFolder);
        }
        
        [Test]
        public void TestCdCanReturnToRoot()
        {
            FileSystemManager.CurrentFolder = (Folder) root.Content["Account1"];
            FileSystemManager.MoveToFolder("*");
            Assert.AreEqual(root, FileSystemManager.CurrentFolder);
        }
        
        [Test]
        public void TestCdCanReturnParentFolder()
        {
            FileSystemManager.CurrentFolder = (Folder) root.Content["Account1"];
            FileSystemManager.MoveToFolder("<");
            Assert.AreEqual(root, FileSystemManager.CurrentFolder);
        }
        
        [Test]
        public void TestCdCanHandleIncorrectPath()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                FileSystemManager.CurrentFolder = (Folder) root.Content["Account1"];
                FileSystemManager.MoveToFolder("sdasdasd");
                Assert.AreEqual((Folder) root.Content["Account1"], FileSystemManager.CurrentFolder);
                Assert.AreEqual("Folder sdasdasd does not exist!\r\n", sw.ToString());
            }
        }
    }

}