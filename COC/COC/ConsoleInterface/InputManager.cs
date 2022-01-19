using System;
using System.Linq;
using System.Windows.Input;
using COC.Application;
using COC.Infrastructure;
using YandexDisk.Client.Http;
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
            if (elements.Length == 1)
                return Regex.Split(line, " +");
            var path = elements[1];
            var result = Regex.Split(elements[0], " +").Concat(new []{path}).Concat(elements[2].Split());
            return result.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
        
        // cd sigmarblessme@gmail.com/yandex/YandexFolder1
        // cd sigmarblessme@gmail.com/dropbox/Folder1
        // download YandexPresentation1.pptx
        // upload F:\Leonid Programmes\COCtest\newTXT.txt
        // upload F:\Leonid Programmes\COCtest\txt2.txt
        // upload F:\Leonid Programmes\COCtest
        // upload F:\Leonid Programmes\HW4 python pair task\public-materials\29-profiling\example_2.log
    }
}