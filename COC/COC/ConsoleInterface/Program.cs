using System;
using System.Linq;
using System.Net.Mime;
using COC.Application;
using COC.Infrastructure;
using File = System.IO.File;

namespace COC.ConsoleInterface
{
    public static class Program
    {
        public static bool isRunning = true;
        private static void Main(string[] args)
        {
            Initialize();
            var dataLoader = new DataLoader(TokenStorage.NameToAccount.Values.ToList());
            dataLoader.InitializeFileSystem();
            while(isRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand();    
            }
        }

        private static void Initialize()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tokens.txt";
            
            Account.DeleteIncorrectLines();
            foreach (var line in File.ReadLines(path))
            {
                if (line == "")
                    continue;
                var data = line.Split(' ');
                if (data.Length != 3)
                    Console.WriteLine($"WARNING: Incorrect format of line \"{line}\". Please, remove it manually in tokens.txt or using .");
                else
                {
                    var token = data[0];
                    var name = data[1];
                    var service = data[2];
                    TokenStorage.AddToken(token, name, service);
                }
            }
            // string token1 = "bqoXZJknL3gAAAAAAAAAAeS2oaOrMkcQg8kKCTITCy9PrBrqJG5xnp3N3xagnHKa";
            // string token2 = "4mUDzkowZAIAAAAAAAAAAb5ko9BO0noUJ0aLye-yVElUpjFGiV1ZwSXwx5gs1FuL";
            // string token3 = "AQAAAABZ-f3MAAd6tWwREaOfEE3Qiv0H4XLT0KY";
            // TokenStorage.AddToken(token1, "Leonid", "dropbox");
            // TokenStorage.AddToken(token2, "Sergei", "dropbox");
            // TokenStorage.AddToken(token3, "Leonid", "yandex");
            // string token4 = TokenStorage.GetToken(TokenStorage.YandexOAuth2);
        }
    }
}