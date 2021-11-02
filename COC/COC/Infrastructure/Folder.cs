using System.Collections.Generic;
using System.Linq;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public Dictionary<string, IFileSystemUnit> Content;

        public static Folder root;
        public string Path { get; set; }
        public string Name { get; set; }

        public Folder PreviousFolder;

        public Folder(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }
        
        public Folder(string path, Folder previousFolder)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            PreviousFolder = previousFolder;
        }

        public Folder(string path, Dictionary<string, IFileSystemUnit> content,  Folder previousFolder=null)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            // MetadataContent = metadataContent;
            Content = content;
            PreviousFolder = previousFolder;
        }

        public void SetContent(Dictionary<string, IFileSystemUnit> content)
        {
            Content = content;
        }

        public static void SetRoot(Folder root)
        {
            Folder.root = root;
        }
    }
}