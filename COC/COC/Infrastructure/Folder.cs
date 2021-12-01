using System.Collections.Generic;
using System.Linq;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public class Folder: IFileSystemUnit
    {
        public Dictionary<string, IFileSystemUnit> Content = new Dictionary<string, IFileSystemUnit>();

        public static Folder root;
        public string Path { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }

        public Folder ParentFolder;

        public Folder(string path)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
        }

        public Folder(string path, Dictionary<string, IFileSystemUnit> content, string mail, Folder parentFolder=null)
        {
            Path = path;
            Name = path.Split('/').LastOrDefault();
            Mail = mail;
            // MetadataContent = metadataContent;
            Content = content;
            ParentFolder = parentFolder;
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