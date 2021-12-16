using System;
using System.Linq;
using COC.Application;
using COC.Infrastructure;
using YandexDisk.Client.Http;

namespace COC.ConsoleInterface
{
    public static class InputManager
    {
        public static void ReadCommand(ref bool isRunning)
        {
            var line = Console.ReadLine().Split('>');
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
            var parameter = "";
            if (line.Length > 1)
                parameter = line[1];
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
                        Downloader.DownloadFile(string.IsNullOrEmpty(argument)
                            ? FileSystemManager.CurrentFolder
                            : FileSystemManager.CurrentFolder.Content[argument]);
                        break;
                        // cd sigmarblessme@gmail.com/yandex/YandexFolder1
                        // cd sigmarblessme@gmail.com/dropbox/Folder1
                        // download YandexPresentation1.pptx
                        // upload F:\Leonid Programmes\COCtest\newTXT.txt
                        // upload F:\Leonid Programmes\COCtest\txt2.txt
                        // upload F:\Leonid Programmes\COCtest
                        // upload F:\Leonid Programmes\HW4 python pair task\public-materials\29-profiling\example_2.log
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        break;
                    }

                case "upload":
                {
                    if (string.IsNullOrEmpty(argument))
                        Console.WriteLine("Write path to the file to upload");
                    else
                        Uploader.UploadFile(FileSystemManager.CurrentFolder, argument);
                    break;
                }

                case "add_account":
                {
                    if (string.IsNullOrEmpty(argument))
                        Console.WriteLine("Write a name of service");
                    else
                    {
                        argument = argument.ToLower();
                        if (argument == "yandex")
                        {
                            var account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.YandexOAuth2), parameter,
                                argument);
                            DataLoader.GetFoldersFromNewAccount(account, argument);
                            // Yandex.YandexDataLoader.GetFolders(account, "",
                            //     new DiskHttpApi(account.ServicesTokens[argument]));
                        }
                        else if (argument.ToLower() == "dropbox")
                        {
                            var account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.DropboxOAuth2), parameter,
                                argument.ToLower());
                            DataLoader.GetFoldersFromNewAccount(account, argument);
                        }
                        else
                            Console.WriteLine("Unsupported service");
                    }
                    break;
                }

                default:
                    Console.WriteLine($"Unknown command \'{command}\'");
                    return;
            }
        }
    }
}