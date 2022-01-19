using System;
using System.Collections.Generic;
using System.IO;
using COC.Infrastructure;
using File = System.IO.File;

namespace COC.Application
{
    public class Account
    {
        private static string pathToTokensTxt = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tokens.txt";
        private static Dictionary<string, Account> accounts = new Dictionary<string, Account>();
        public readonly string AccountName;
        public readonly Dictionary<string, string> ServicesTokens;

        public Account(string accountName)
        {
            AccountName = accountName;
            ServicesTokens = new Dictionary<string, string>();
        }

        public static void AddAccount(string serviceName, string accountName)
        {
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
            accounts.Add(accountName, account);
            using (StreamWriter sw = File.AppendText(pathToTokensTxt))
            {
                sw.WriteLine(account.ServicesTokens[serviceName] + ' ' + account.AccountName + ' ' + serviceName);
            }
        }

        public static void DeleteService(string serviceName, string accountName)
        {
            serviceName = serviceName.ToLower();
            if (Folder.Root.Content.ContainsKey(accountName))
            {
                if (Folder.Root.Content[accountName] is Folder && ((Folder)Folder.Root.Content[accountName]).Content.ContainsKey(serviceName))
                    ((Folder)Folder.Root.Content[accountName]).Content.Remove(serviceName);
            }
            else
            {
                Console.WriteLine("Account was not added");
                return;
            }

            var newLines = new List<string>();
            using (StreamReader reader = new StreamReader(pathToTokensTxt))
            {
                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    if (line == "")
                        continue;
                    if (line.Split().Length != 3)
                        continue;
                    if (line.Split()[1] == accountName && line.Split()[2] == serviceName)
                        continue;
                    newLines.Add(line);
                }
            }

            using (StreamWriter writer = File.CreateText(pathToTokensTxt))
            {
                foreach (var line in newLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public static void DeleteAccount(string accountName)
        {
            if (Folder.Root.Content.ContainsKey(accountName))
            {
                Folder.Root.Content.Remove(accountName);
            }
            else
            {
                Console.WriteLine("Account was not added");
                return;
            }

            var newLines = new List<string>();
            using (StreamReader reader = new StreamReader(pathToTokensTxt))
            {
                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    if (line == "")
                        continue;
                    if (line.Split().Length != 3)
                        continue;
                    if (line.Split()[1] == accountName)
                        continue;
                    newLines.Add(line);
                }
            }

            using (StreamWriter writer = File.CreateText(pathToTokensTxt))
            {
                foreach (var line in newLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public static void DeleteIncorrectLines()
        {
            var newLines = new List<string>();
            using (StreamReader reader = new StreamReader(pathToTokensTxt))
            {
                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    if (line == "")
                        continue;
                    if (line.Split().Length != 3)
                        continue;
                    newLines.Add(line);
                }
            }
            using (StreamWriter writer = File.CreateText(pathToTokensTxt))
            {
                foreach (var line in newLines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}