using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Application;
using COC.ConsoleInterface;
using COC.Infrastructure;
using Ninject;
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
                var file = UploadSingleFile(pathToUpload, fileToUploadPath, yandexClient, account);
                FileSystemManager.CurrentFolder.Content.Add(name, file);
            }

            if (Directory.Exists(fileToUploadPath))
            {
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
            var pathArg =
                new Ninject.Parameters.ConstructorArgument("path", $"Root/{account.AccountName}/yandex{pathToUpload}");
            var contentArg =
                new Ninject.Parameters.ConstructorArgument("content", new Dictionary<string, IFileSystemUnit>());
            var accountArg = new Ninject.Parameters.ConstructorArgument("account", account);
            var localFolder = Program.container.Get<Folder>(pathArg, contentArg, accountArg);
            localFolder.ParentFolder = parentFolder;
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

        private Infrastructure.File UploadSingleFile(string pathToUpload, string fileToUploadPath, DiskHttpApi client,
            Account account)
        {
            var file = File.Open(fileToUploadPath, FileMode.Open, FileAccess.Read);
            var name = fileToUploadPath.Split('\\').Last();
            Console.WriteLine("Uploading " + name);
            Task.Run(() => client.Files.UploadFileAsync(pathToUpload, false, file));
            return new Infrastructure.File(Infrastructure.FileSystemManager.CurrentFolder.Path + '/' + name, account);
        }
        //cd Leonid/yandex/YandexFolder1
        //upload F:\COCtest


        // public static long DirSize(DirectoryInfo d) 
        // {    
        //     long size = 0;    
        //     // Add file sizes.
        //     FileInfo[] fis = d.GetFiles();
        //     foreach (FileInfo fi in fis) 
        //     {      
        //         size += fi.Length;    
        //     }
        //     // Add subdirectory sizes.
        //     DirectoryInfo[] dis = d.GetDirectories();
        //     foreach (DirectoryInfo di in dis) 
        //     {
        //         size += DirSize(di);   
        //     }
        //     return size;  
        // }
    }
}