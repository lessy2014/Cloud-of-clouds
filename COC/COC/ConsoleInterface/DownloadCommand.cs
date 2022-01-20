using System;
using COC.Application;
using COC.Domain;
using CommandLine;


namespace COC.ConsoleInterface
{
    [Verb("download", HelpText = "Download file or folder")]
    public class DownloadCommand: ICommand
    {
        [Value(index: 0, Required = false, HelpText = "Name of file or folder")]
        public string objectName { get; set; }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(objectName))
                if (FileSystemManager.CurrentFolder.Content.ContainsKey(objectName))
                    Downloader.DownloadFile(FileSystemManager.CurrentFolder.Content[objectName]);
                else
                {
                    Console.WriteLine("This file or folder does not exist!");
                }
            else
                Downloader.DownloadFile(FileSystemManager.CurrentFolder);
        }
    }
}