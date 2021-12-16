using System;
using System.Diagnostics;
using System.Windows.Input;
using COC.Infrastructure;
using CommandLine;
using Newtonsoft.Json;

namespace COC.ConsoleInterface
{
    [Verb("cd", HelpText = "Move to another folder")]
    public class CdCommand: ICommand
    {
        [Value(index: 0, Required = true, HelpText = "Path to another folder")]
        public string Path { get; set; }
        
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            FileSystemManager.MoveToFolder(Path);
        }

        public event EventHandler CanExecuteChanged;
    }
}