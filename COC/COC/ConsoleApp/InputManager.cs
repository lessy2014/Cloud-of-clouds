using System;
using System.Collections.Generic;
using System.Linq;
using COC.Dropbox;
using COC.Infrastructure;

namespace COC.ConsoleApp
{
    public static class InputManager
    {
        public static void ReadCommand(ref bool isRunning, Dictionary<string, Dictionary<string, string>> mailToToken)
        {
            var line = Console.ReadLine().Split(':');
            if (line.Length > 2)
            {
                Console.WriteLine("Wrong number of parameters");
                return;
            }

            var request = line[0].Split();
            var command = request[0];
            string argument = null;
            if (request.Length > 1)
                argument = string.Join(" ", request.Skip(1));
            string[] parameters = null;
            if (line.Length == 2)
                parameters = line[1].Split();
            // var args = new List<string>(new ArraySegment<string>(line, 1, line.Length-1));
            switch (command)
            {
                case "dir":
                    OutputManager.WriteFolderData(FileSystemManager.CurrentFolder);
                    return;
                case "cd":
                    try
                    {
                        FileSystemManager.MoveToFolder(argument);
                        return;
                    }
                    catch
                    {
                        Console.WriteLine("no");
                        break;
                    }

                case "close":
                    isRunning = false;
                    return;
                
                case "download":
                    try
                    {
                        if (argument == null)
                            Downloader.DownloadFile(FileSystemManager.CurrentFolder, mailToToken);
                        else
                            Downloader.DownloadFile(FileSystemManager.CurrentFolder.Content[argument], mailToToken); //TODO нельзя скачать файл с пробелом в названии
                        break;
                        // cd sigmarblessme@gmail.com/yandex/YandexFolder1
                        // download YandexPresentation1.pptx
                    }
                    catch
                    {
                        Console.WriteLine("no");
                        break;
                    }
                    
                default:
                    Console.WriteLine($"Unknown command \'{command}\'");
                    return;
            }
        }
    }
}