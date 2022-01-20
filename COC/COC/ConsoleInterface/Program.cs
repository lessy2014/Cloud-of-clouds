using System;
using COC.Application;
using COC.Domain;
using Ninject;

namespace COC.ConsoleInterface
{
    public static class Program
    {
        public static bool IsRunning = true;
        public static StandardKernel Container;
        private static void Main(string[] args)
        {
            Container = App.ConfigureContainer();
            App.Start();
            while(IsRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand();    
            }
        }
    }
}