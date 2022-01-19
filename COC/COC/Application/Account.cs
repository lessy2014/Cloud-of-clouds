using System;
using System.Collections.Generic;
using System.IO;

namespace COC.Application
{
    public class Account
    {
        public readonly string AccountName;
        public readonly Dictionary<string, string> ServicesTokens;

        public Account(string accountName)
        {
            AccountName = accountName;
            ServicesTokens = new Dictionary<string, string>();
        }

        public static void AddAccount(string serviceName, string accountName)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tokens.txt";
            Account account = null;
            serviceName = serviceName.ToLower();
            string token;
            switch (serviceName)
            {
                case "yandex":
                    token = TokenStorage.GetToken(TokenStorage.YandexOAuth2);
                    break;
                case "dropbox":
                    token = TokenStorage.GetToken(TokenStorage.DropboxOAuth2);
                    break;
                default:
                    Console.WriteLine("Unsupported service");
                    return;
            }
            if (token == "")
                return;
            account = TokenStorage.AddToken(token, accountName, serviceName.ToLower());
            DataLoader.GetFoldersFromNewAccount(account, serviceName);
            if (account == null) return;
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(account.ServicesTokens[serviceName] + ' ' + account.AccountName + ' ' + serviceName);
            }
        }
    }
}