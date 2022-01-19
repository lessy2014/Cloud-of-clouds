using System;
using COC.Application;
using COC.Infrastructure;
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
            // cd sigmarblessme@gmail.com/yandex/YandexFolder1
                // cd sigmarblessme@gmail.com/dropbox/Folder1
                // download YandexPresentation1.pptx
                // upload F:\Leonid Programmes\COCtest\newTXT.txt
                // upload F:\Leonid Programmes\COCtest\txt2.txt
                // upload F:\Leonid Programmes\COCtest
                // upload F:\Leonid Programmes\HW4 python pair task\public-materials\29-profiling\example_2.log
        }
    }
}