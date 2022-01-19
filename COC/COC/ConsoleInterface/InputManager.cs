using System;
using System.Linq;
using CommandLine;
using System.Text.RegularExpressions;

namespace COC.ConsoleInterface
{
    public static class InputManager
    { 
        public static void ReadCommand()
        {
            var command = Console.ReadLine().Trim();
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
            if (line.LastOrDefault() == '/')
                line = line.Remove(line.Length - 1);
            if (elements.Length == 1)
                return Regex.Split(line, " +");
            var path = elements[1];
            var result = Regex.Split(elements[0], " +").Concat(new []{path}).Concat(elements[2].Split());
            return result.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}