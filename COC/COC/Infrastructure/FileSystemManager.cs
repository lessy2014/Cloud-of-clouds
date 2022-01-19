using System;

namespace COC.Infrastructure
{
    public static class FileSystemManager
    {
        private static string CurrentPath = "";
        public static Folder CurrentFolder;
        public static int AccountsCount;

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
                    if (CheckFolderExistence(folder))
                        CurrentFolder = (Folder) CurrentFolder.Content[folder];
                    else
                    {
                        CurrentFolder = tempFolder;
                        return;
                    }
                }
            }
            else
            {
                CurrentFolder = Folder.Root;
                if (CheckFolderExistence(splittedPath[1]))
                    CurrentFolder = (Folder) Folder.Root.Content[splittedPath[1]];
                else
                {
                    CurrentFolder = tempFolder;
                    return;
                }
                for (var i = 2; i < splittedPath.Length; i++)
                {
                    if (CheckFolderExistence(splittedPath[i]))
                        CurrentFolder = (Folder)CurrentFolder.Content[splittedPath[i]];
                    else
                    {
                        CurrentFolder = tempFolder;
                        return;
                    }
                }
            }
        }

        private static bool CheckFolderExistence(string folder)
        {
            if (CurrentFolder.Content.ContainsKey(folder))
                if (CurrentFolder.Content[folder] is Folder)
                    return true;
                else
                {
                    Console.WriteLine($"{folder} in path is a file!");
                    return false;
                }
            Console.WriteLine($"Folder {folder} does not exist!");
            return false;
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