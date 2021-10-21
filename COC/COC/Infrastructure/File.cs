using System.Linq;

namespace COC.Infrastructure
{
    public class File: IFileSystemUnit
    {
        public string Path { get; set; }
        public string Name { get; set; }
        
        public File(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }

    }
}