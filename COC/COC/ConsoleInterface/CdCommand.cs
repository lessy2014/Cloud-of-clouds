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
        
        public void Execute()
        {
            FileSystemManager.MoveToFolder(Path);
        }
    }
}