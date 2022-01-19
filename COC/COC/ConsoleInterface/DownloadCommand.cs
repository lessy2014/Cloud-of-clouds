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
            try
            {
                Downloader.DownloadFile(string.IsNullOrEmpty(objectName)
                    ? FileSystemManager.CurrentFolder
                    : FileSystemManager.CurrentFolder.Content[objectName]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}