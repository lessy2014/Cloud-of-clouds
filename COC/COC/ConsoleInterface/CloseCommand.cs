using System;
using System.Windows.Input;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("close", HelpText = "Close the program")]
    public class CloseCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            Program.isRunning = false;
        }

        public event EventHandler CanExecuteChanged;
    }
}