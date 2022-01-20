using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Application;
using COC.Domain;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Yandex
{
    public class YandexUploader : IUploader
    {
        public void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var yandexClient = new DiskHttpApi(account.ServicesTokens["yandex"]);
            var name = fileToUploadPath.Split('\\').Last();
            if (File.Exists(fileToUploadPath))
            {
                if (FileSystemManager.CurrentFolder.Content.ContainsKey(name))
                {
                    Console.WriteLine("File with this name already exists. You can delete it manually and then upload, rename it or upload in another folder.");
                    return;
                }
                var file = UploadSingleFile(pathToUpload, fileToUploadPath, yandexClient, account);
                FileSystemManager.CurrentFolder.Content.Add(name, file);
            }

            if (Directory.Exists(fileToUploadPath))
            {
                if (FileSystemManager.CurrentFolder.Content.ContainsKey(name))
                {
                    Console.WriteLine("Folder with this name already exists. You can delete it manually and then upload, rename it or upload in another folder.");
                    return;
                }
                var folder = UploadFolder(pathToUpload, fileToUploadPath, yandexClient, account,
                    FileSystemManager.CurrentFolder);
                if (folder != null)
                    FileSystemManager.CurrentFolder.Content.Add(name, folder);
            }
        }

        private Folder UploadFolder(string pathToUpload, string fileToUploadPath, DiskHttpApi client, Account account,
            Folder parentFolder)
        {
            var directory = client.Commands.CreateDictionaryAsync(pathToUpload).Result;
            var localFolder = new Folder($"Root/{account.AccountName}/yandex{pathToUpload}",
                new Dictionary<string, IFileSystemUnit>(), account)
            {
                ParentFolder = parentFolder
            };
            foreach (var subdirectory in Directory.GetDirectories(fileToUploadPath))
            {
                var name = subdirectory.Split('\\').Last();
                var subFolder = UploadFolder(pathToUpload + '/' + name, subdirectory, client, account, localFolder);
                localFolder.Content.Add(name, subFolder);
            }

            foreach (var file in Directory.GetFiles(fileToUploadPath))
            {
                var name = file.Split('\\').Last();
                var localFile = UploadSingleFile(pathToUpload + '/' + name, file, client, account);
                localFolder.Content.Add(name, localFile);
            }

            return localFolder;
        }

        private Domain.File UploadSingleFile(string pathToUpload, string fileToUploadPath, DiskHttpApi client,
            Account account)
        {
            var file = File.Open(fileToUploadPath, FileMode.Open, FileAccess.Read);
            var name = fileToUploadPath.Split('\\').Last();
            Console.WriteLine("Uploading " + name);
            Task.Run(() => client.Files.UploadFileAsync(pathToUpload, false, file));
            return new Domain.File(Domain.FileSystemManager.CurrentFolder.Path + '/' + name, account);
        }
    }
}