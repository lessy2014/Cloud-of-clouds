using System;
using System.IO;
using System.Linq;
using COC.Application;
using COC.Infrastructure;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Yandex
{
    public static class YandexUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, Account account)
        {
            var yandexClient = new DiskHttpApi(account.ServicesTokens["yandex"]);
            var file = File.Open(fileToUploadPath, FileMode.Open, FileAccess.Read);
            Task.Run(() => yandexClient.Files.UploadFileAsync(pathToUpload, false, file));
            var name = fileToUploadPath.Split('\\').Last();
            if (name.Split('.').Length == 2)
                FileSystemManager.CurrentFolder.Content.Add(name, new Infrastructure.File(pathToUpload, account));
            else
                YandexDataLoader.GetFolders(account, pathToUpload, yandexClient);
            // YandexDataLoader.GetFolders(account, pathToUpload, yandexClient);
            //             if (metadata.IsFile)
            //     FileSystemManager.CurrentFolder.Content.Add(metadata.Name, new Infrastructure.File(pathToUpload, account));
            // else
            //     DropboxDataLoader.GetFolders(account, pathToUpload, dropboxClient);
        }
    }
}