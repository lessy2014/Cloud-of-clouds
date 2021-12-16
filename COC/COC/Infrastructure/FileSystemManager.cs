using System.Collections.Generic;
using Dropbox.Api.Files;

namespace COC.Infrastructure
{
    public static class FileSystemManager
    {
        private static string CurrentPath = "";
        public static Folder CurrentFolder;

        public static Folder GetFolder(string path)
        {
            if (path == "")
                return Folder.Root;
            var splittedPath = SplitPath(path);
            var folder = (Folder)Folder.Root.Content[splittedPath[0]];
            for (var i = 1; i < splittedPath.Length; i++)
            {
                folder = (Folder)folder.Content[splittedPath[i]];
            }

            return folder;
        }
        
        public static void MoveToFolder(string path)
        {
            switch (path)
            {
                case "":
                    return;
                case "<":
                    CurrentFolder = CurrentFolder.ParentFolder;
                    return;
                case "*":
                    CurrentFolder = Folder.Root;
                    return;
            }

            // path = $"{CurrentFolder.Path}/{path}";
            var tempFolder = CurrentFolder;
            var splittedPath = SplitPath(path);
            if (splittedPath[0] != "*")
            {
                foreach (var folder in splittedPath)
                {
                    CurrentFolder = (Folder) CurrentFolder.Content[folder];
                }
            }
            else
            {
                CurrentFolder = (Folder)Folder.Root.Content[splittedPath[1]];
                for (var i = 2; i < splittedPath.Length; i++)
                {
                    CurrentFolder = (Folder)CurrentFolder.Content[splittedPath[i]];
                }
            }
        }

        private static string[] SplitPath(string path)
        {
            if (path[0] != '/')
                CurrentPath = path;
            else
                CurrentPath += path;
            return CurrentPath.Split('/');
        }
        
    }
}