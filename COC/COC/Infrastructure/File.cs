using System.Linq;

namespace COC.Infrastructure
{
    public class File: IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }

        public File(string path, string mail)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Mail = mail;
        }

    }
}