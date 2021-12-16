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
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        [Value(index:0, Required = true)]
        public string PathToFile { get; set; }

        public void Execute(object parameter)
        { 
            Uploader.UploadFile(FileSystemManager.CurrentFolder, PathToFile);
        }

        public event EventHandler CanExecuteChanged;
    }
}