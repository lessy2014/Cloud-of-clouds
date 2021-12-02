using System.Linq;
using COC.Application;

namespace COC.Infrastructure
{
    public class File: IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        
        public Account Account { get; }

        public File(string path, Account account)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Mail = account.AccountName;
            Account = account;
        }

    }
}