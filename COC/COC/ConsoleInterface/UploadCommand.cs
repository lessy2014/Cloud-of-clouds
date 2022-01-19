using System;
using System.Windows.Input;
using COC.Application;
using COC.Infrastructure;
using CommandLine;


namespace COC.ConsoleInterface
{
    [Verb("upload", HelpText = "Upload file")]
    public class UploadCommand: ICommand
    {
        [Value(index:0, Required = true, HelpText = "Path to file")]
        public string PathToFile { get; set; }

        public void Execute()
        { 
            Uploader.UploadFile(FileSystemManager.CurrentFolder, PathToFile, getUploader());
        }
    }
}