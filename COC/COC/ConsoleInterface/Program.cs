using System;
using System.Linq;
using COC.Application;
using COC.Infrastructure;

namespace COC.ConsoleInterface
{
    public static class Program
    {
        private static bool isRunning = true;
        private static void Main(string[] args)
        {
            Initialize();
            var dataLoader = new DataLoader(TokenStorage.MailToAccount.Values.ToList());
            dataLoader.InitializeFileSystem();
            while(isRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand(ref isRunning);    
            }
        }

        private static void Initialize()
        {
            string token1 = "bqoXZJknL3gAAAAAAAAAAeS2oaOrMkcQg8kKCTITCy9PrBrqJG5xnp3N3xagnHKa";
            string token2 = "4mUDzkowZAIAAAAAAAAAAb5ko9BO0noUJ0aLye-yVElUpjFGiV1ZwSXwx5gs1FuL";
            string token3 = "AQAAAABZ-f3MAAd6tWwREaOfEE3Qiv0H4XLT0KY";
            TokenStorage.AddToken(token1, "dropbox");
            TokenStorage.AddToken(token2, "dropbox");
            TokenStorage.AddToken(token3, "yandex");
        }
    }
}