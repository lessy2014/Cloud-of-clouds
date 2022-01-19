using System;
using COC.Application;
using COC.Infrastructure;
using Ninject;

namespace COC.ConsoleInterface
{
    public static class Program
    {
        public static bool isRunning = true;
        public static StandardKernel container;
        private static void Main(string[] args)
        {
            container = App.ConfigureContainer();
            App.Start();
            while(isRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand();    
            }
        }
    }
}