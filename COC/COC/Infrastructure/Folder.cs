using System.Collections.Generic;
using System.Linq;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public List<Metadata> MetadataContent;

        public Dictionary<string, IFileSystemUnit> Content;

        public static Dictionary<string, Folder> root;
        public string Path { get; set; }
        public string Name { get; set; }

        public Folder(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }

        public Folder(string path, List<Metadata> metadataContent, Dictionary<string, IFileSystemUnit> content)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            MetadataContent = metadataContent;
            Content = content;
        }

        public void SetContent(Dictionary<string, IFileSystemUnit> content)
        {
            Content = content;
        }

        public static void SetRoot(Dictionary<string, Folder> root)
        {
            Folder.root = root;
        }
    }
}