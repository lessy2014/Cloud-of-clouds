using System;
using System.Windows.Input;
using COC.Infrastructure;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("dir", HelpText = "Show content of current folder")]
    public class DirCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            OutputManager.WriteFolderData(FileSystemManager.CurrentFolder);
        }

        public event EventHandler CanExecuteChanged;
    }
}