using System.Collections.Generic;
using System.Linq;
using COC.Application;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public Dictionary<string, IFileSystemUnit> Content = new Dictionary<string, IFileSystemUnit>();

        public static Folder Root;
        public string Path { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        
        public Account Account { get; }

        public Folder ParentFolder;

        public Folder(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }
        
        public Folder(string path, Dictionary<string, IFileSystemUnit> content)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Content = content;
        }

        public Folder(string path, Dictionary<string, IFileSystemUnit> content, Account account)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Mail = account.AccountName;
            Content = content;
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