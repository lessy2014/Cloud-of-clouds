using System;
using System.Windows.Input;
using COC.Infrastructure;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("dir", HelpText = "Show content of current folder")]
    public class DirCommand: ICommand
    {
        public void Execute()
        {
            OutputManager.WriteFolderData(FileSystemManager.CurrentFolder);
        }
    }
}