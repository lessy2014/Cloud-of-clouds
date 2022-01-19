using System;
using System.Windows.Input;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("close", HelpText = "Close the program")]
    public class CloseCommand: ICommand
    {
        public void Execute()
        {
            Program.isRunning = false;
        }
    }
}