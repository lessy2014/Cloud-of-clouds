using System;
using System.Collections.Generic;
using COC.Dropbox;
using COC.Domain;
using COC.Yandex;

namespace COC.Application
{
    public class DataLoader
    {
        private readonly List<Account> accounts;
        private IDataLoader dataLoader;

        public void InitializeFileSystem()
        {
            var root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            foreach (var account in accounts)
            {
                var accountFolder = new Folder($"Root/{account.AccountName}")
                {
                    Account = account
                };
                foreach (var serviceToken in account.ServicesTokens)
                {
                    try
                    {
                        dataLoader = serviceToken.Key == "dropbox"? new DropboxDataLoader(): new YandexDataLoader();
                        accountFolder.Content.Add(serviceToken.Key,
                            GetFolders(account, serviceToken.Value, dataLoader));
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine(
                            $"WARNING: Probably, account {account.AccountName} has invalid token for {serviceToken.Key}. Delete it manually in tokens.txt by removing corresponding line or using delete_account command.");
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"WARNING: Account {account.AccountName} uses unsupported service {serviceToken.Key}. Delete it manually in tokens.txt by removing corresponding line or using delete_account command.");
                    }
                }

                accountFolder.ParentFolder = root;
                foreach (var folder in accountFolder.Content.Values)
                {
                    ((Folder) folder).ParentFolder = accountFolder;
                }

                root.Content.Add(accountFolder.Name, accountFolder);
            }

            Folder.SetRoot(root);
            root.ParentFolder = root;
            FileSystemManager.CurrentFolder = root;
        }

        private static Folder GetFolders(Account account, string token, IDataLoader dataLoader)
        {
            return dataLoader.GetFolders(account, "", token);
        }
        
        public static void GetFoldersFromNewAccount(Account account, string service)
        {
            IDataLoader dataLoader = service == "dropbox"? new DropboxDataLoader(): new YandexDataLoader();
            Folder mailFolder;
            if (Folder.Root.Content.ContainsKey(account.AccountName))
            {
                mailFolder = (Folder)Folder.Root.Content[account.AccountName];
            }
            else
            {
                mailFolder = new Folder($"Root/{account.AccountName}") {ParentFolder = Folder.Root};
                Folder.Root.Content.Add(mailFolder.Name, mailFolder);
            }
            mailFolder.Content.Add(service, GetFolders(account, account.ServicesTokens[service], dataLoader));
            
            foreach (var folder in mailFolder.Content.Values)
            {
                ((Folder) folder).ParentFolder = mailFolder;
            }
        }
        
        public DataLoader(List<Account> accounts)
        {
            this.accounts = accounts;
        } 
    }
}