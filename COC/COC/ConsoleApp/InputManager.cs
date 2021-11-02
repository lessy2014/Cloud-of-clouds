using System;
using System.Collections.Generic;
using COC.Infrastructure;

namespace COC.ConsoleApp
{
    public static class InputManager
    {
        public static void ReadCommand(ref bool isRunning)
        {
            var line = Console.ReadLine().Split();
            var command = line[0];
            var args = new List<string>(new ArraySegment<string>(line, 1, line.Length-1));
            switch (command)
            {
                case "dir":
                    OutputManager.WriteFolderData(FileSystemManager.CurrentFolder);
                    return;
                case "cd":
                    try
                    {
                        FileSystemManager.MoveToFolder(args[0]);
                        return;
                    }
                    catch
                    {
                        Console.WriteLine("no");
                        break;
                    }

                case "close":
                    isRunning = false;
                    return;
                default:
                    Console.WriteLine($"Unknown command \'{command}\'");
                    return;
            }
        }
    }
}