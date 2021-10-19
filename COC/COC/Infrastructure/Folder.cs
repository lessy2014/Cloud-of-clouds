using System.Collections.Generic;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public string Path;
        public string Name;
        public List<Metadata> Content;

        public Folder(string path, List<Metadata> content)
        {
            Path = path;
            Content = content;
        }
    }
}