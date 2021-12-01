using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dropbox.Api;
using System.Threading.Tasks;
using COC.Infrastructure;
using Dropbox.Api.Files;
using Dropbox.Api.Users;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using Account = COC.Application.Account;


namespace COC
{
    public class DataLoader //TODO рефактор всего класса
    {
        private readonly List<Account> accounts;

        public void InitializeFileSystem()
        {
            var root = new Folder("Root", new Dictionary<string, IFileSystemUnit>());
            foreach (var account in accounts)
            {
                var mailFolder = new Folder($"Root/{account.Mail}");
                foreach (var serviceToken in account.ServicesTokens)
                {
                    mailFolder.Content.Add(serviceToken.Key,
                        GetFolders(account, serviceToken.Key, serviceToken.Value));
                }

                mailFolder.ParentFolder = root;
                foreach (var folder in mailFolder.Content.Values)
                {
                    ((Folder) folder).ParentFolder = mailFolder;
                }

                root.Content.Add(mailFolder.Name, mailFolder);
            }

            Folder.SetRoot(root);
            FileSystemManager.CurrentFolder = root;
        }

        private static Folder GetFolders(Account account, string service, string token)
        {
            switch (service)
            {
                case "yandex":
                {
                    var client = new DiskHttpApi(token);
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
        
        public DataLoader(List<Account> accounts)
        {
            this.accounts = accounts;
        } 
    }
}