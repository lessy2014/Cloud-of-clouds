using System;
using System.Collections.Generic;
using System.Threading;
using COC.Dropbox;
using COC.Infrastructure;
using COC.Yandex;
using Dropbox.Api;
using Dropbox.Api.TeamLog;
using YandexDisk.Client.Http;


namespace COC.Application
{
    public class DataLoader
    {
        private readonly List<Account> accounts;

        public void InitializeFileSystem()
        {
            var root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            foreach (var account in accounts)
            {
                var mailFolder = new Folder($"Root/{account.AccountName}");
                foreach (var serviceToken in account.ServicesTokens)
                {
                    try
                    {
                        mailFolder.Content.Add(serviceToken.Key,
                            GetFolders(account, serviceToken.Key, serviceToken.Value));
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine(
                            $"WARNING: Probably, account {account.AccountName} has invalid token for {serviceToken.Key}. Delete it manually in tokens.txt by removing corresponding line.");
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"WARNING: Account {account.AccountName} using unsupported service {serviceToken.Key}. Delete it manually in tokens.txt by removing corresponding line.");
                    }
                }

                mailFolder.ParentFolder = root;
                foreach (var folder in mailFolder.Content.Values)
                {
                    ((Folder) folder).ParentFolder = mailFolder;
                }

                root.Content.Add(mailFolder.Name, mailFolder);
            }

            Folder.SetRoot(root);
            root.ParentFolder = root;
            FileSystemManager.CurrentFolder = root;
        }

        private static Folder GetFolders(Account account, string service, string token)
        {
            switch (service)
            {
                case "yandex":
                {
                    DiskHttpApi client = null;
                    client = new DiskHttpApi(token);
                    return YandexDataLoader.GetFolders(account, "", client);
                }
                case "dropbox":
                {
                    var dropboxClient = new DropboxClient(token);
                    return DropboxDataLoader.GetFolders(account, "", dropboxClient);
                }
                default:
                    throw new ArgumentException("Unknown service");
            }
        }
        
        public static void GetFoldersFromNewAccount(Account account, string service)
        {
            var mailFolder = new Folder($"Root/{account.AccountName}");
            mailFolder.Content.Add(service, GetFolders(account, service, account.ServicesTokens[service]));
            

            mailFolder.ParentFolder = Folder.Root;
            foreach (var folder in mailFolder.Content.Values)
            {
                ((Folder) folder).ParentFolder = mailFolder;
            }

            Folder.Root.Content.Add(mailFolder.Name, mailFolder);
        }
        
        public DataLoader(List<Account> accounts)
        {
            this.accounts = accounts;
        } 
    }
}