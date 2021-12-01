using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using COC.Dropbox;
using COC.Infrastructure;
using Dropbox.Api.Files;
using Dropbox.Api.Users;

namespace COC
{
    public class Program
    {
        private static bool isRunning = true;
        private static void Main(string[] args)
        {
            Initialize();
            var dataLoader = new DataLoader(TokenStorage.mailToAccount.Values.ToList());
            dataLoader.InitializeFileSystem();
            while(isRunning)
            {
                Console.Write($"{FileSystemManager.CurrentFolder.Path}> ");
                InputManager.ReadCommand(ref isRunning, TokenStorage.MailToToken);    
            }
        }

        private static void Initialize()
        {
            string token1 = "bqoXZJknL3gAAAAAAAAAAeS2oaOrMkcQg8kKCTITCy9PrBrqJG5xnp3N3xagnHKa";
            string token2 = "4mUDzkowZAIAAAAAAAAAAb5ko9BO0noUJ0aLye-yVElUpjFGiV1ZwSXwx5gs1FuL";
            string token3 = "AQAAAABZ-f3MAAd6tWwREaOfEE3Qiv0H4XLT0KY";
            TokenStorage.AddDropboxToken(token1);
            TokenStorage.AddDropboxToken(token2);
            TokenStorage.AddYandexToken(token3);
            TokenStorage.AddToken(token1, "dropbox");
            TokenStorage.AddToken(token2, "dropbox");
            TokenStorage.AddToken(token3, "yandex");
        }
    }
}