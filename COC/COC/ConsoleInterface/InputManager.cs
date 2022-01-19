using System;
using System.Linq;
using CommandLine;

namespace COC.ConsoleInterface
{
    public static class InputManager
    { 
        public static void ReadCommand()
        {
            var command = Console.ReadLine();
            Parser.Default.ParseArguments<
                    CdCommand, 
                    DirCommand, 
                    CloseCommand,
                    DownloadCommand,
                    UploadCommand,
                    AddAccountCommand,
                    DeleteAccountCommand,
                    DeleteServiceCommand,
                    FindCommand
                >(HandleInput(command))
                .WithParsed<ICommand>(command1 => command1.Execute());
        }

        private static string[] HandleInput(string line)
        {
            var elements = line.Split('\"');
            if (elements.Length == 1)
                return line.Split();
            var path = elements[1];
            var result = elements[0].Split().Concat(new []{path}).Concat(elements[2].Split());
            return result.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}