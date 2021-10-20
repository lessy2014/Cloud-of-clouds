using System.Collections.Generic;
using System.Linq;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public List<Metadata> Content;

        public Folder(string path, List<Metadata> content)
        {
            Path = path;
            // TODO имя папки
            // Name = path.Split('/').LastOrDefault();
            Content = content;
        }
    }
}