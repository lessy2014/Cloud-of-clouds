using System.Collections.Generic;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public static class FileSystemManager
    {
        public static string CurrentPath;

        public static Folder GetFolder(string path)
        {
            if (path == "")
                return Folder.root;
            var splittedPath = SplitPath(path);
            var folder = (Folder)Folder.root.Content[splittedPath[0]];
            for (var i = 1; i < splittedPath.Length; i++)
            {
                folder = (Folder)folder.Content[splittedPath[i]];
            }

            return folder;

        }

        public static string[] SplitPath(string path)
        {
            if (path[0] != '/')
                CurrentPath = path;
            else
                CurrentPath += path;
            return CurrentPath.Split('/');
        }
        
    }
}