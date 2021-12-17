using System;
using System.Linq;
using System.Windows.Input;
using COC.Application;
using COC.Infrastructure;
using YandexDisk.Client.Http;
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
                    FindCommand
                >(HandleInput(command))
                .WithParsed<ICommand>(command1 => command1.Execute(null));
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
        
        // cd sigmarblessme@gmail.com/yandex/YandexFolder1
        // cd sigmarblessme@gmail.com/dropbox/Folder1
        // download YandexPresentation1.pptx
        // upload F:\Leonid Programmes\COCtest\newTXT.txt
        // upload F:\Leonid Programmes\COCtest\txt2.txt
        // upload F:\Leonid Programmes\COCtest
        // upload F:\Leonid Programmes\HW4 python pair task\public-materials\29-profiling\example_2.log
    }
}