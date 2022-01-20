using System.Collections.Generic;
using System.Linq;
using COC.Application;
using COC.ConsoleInterface;
using Ninject;

namespace COC.Domain
{
    public class Folder : IFileSystemUnit
    {
        public Dictionary<string, IFileSystemUnit> Content = new Dictionary<string, IFileSystemUnit>();

        public static Folder Root;
        public string Path { get; set; }
        public string Name { get; set; }
        public Account Account { get; set; }

        public Folder ParentFolder;

        public IService Service { get; set; }

        public Folder(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }

        public Folder(string path, Dictionary<string, IFileSystemUnit> content)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            if(path.Split('/').Length>2)
                Service = path.Split('/')[2] == "dropbox" ? Program.Container.Get<Dropbox>() : Program.Container.Get<Yandex>();
            Content = content;
        }

        public Folder(string path, Dictionary<string, IFileSystemUnit> content, Account account)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Content = content;
            if(path.Split('/').Length>2)
                Service = path.Split('/')[2] == "dropbox" ? Program.Container.Get<Dropbox>() : Program.Container.Get<Yandex>();

            Account = account;
        }

        public void SetContent(Dictionary<string, IFileSystemUnit> content)
        {
            Content = content;
        }

        public static void SetRoot(Folder root)
        {
            Folder.Root = root;
        }
    }
}