using System.Linq;
using COC.Application;
using COC.ConsoleInterface;
using Ninject;

namespace COC.Infrastructure
{
    public class File : IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public Account Account { get; }
        public IService Service { get; set; }
        
        public File(string path, Account account)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Service = path.Split('/')[2] == "dropbox" ? Program.container.Get<Dropbox>() : Program.container.Get<Yandex>();
            Account = account;
        }
    }
}