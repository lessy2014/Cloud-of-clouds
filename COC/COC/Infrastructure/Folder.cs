using System.Collections.Generic;
using System.Linq;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public List<Metadata> Content;
        public string Path { get; set; }
        public string Name { get; set; }

        public Folder(string path, List<Metadata> content)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Content = content;
        }
    }
}