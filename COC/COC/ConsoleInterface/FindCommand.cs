using System;
using System.Windows.Input;
using COC.Application;
using COC.Infrastructure;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("find", HelpText = "Find all files and folders with given name")]
    public class FindCommand: ICommand
    {
        [Value(index:0, Required = true, HelpText = "Name of file or Folder")]
        public string Name { get; set; }
        
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            Finder.Find(FileSystemManager.CurrentFolder, Name);
        }

        public event EventHandler CanExecuteChanged;
    }
}