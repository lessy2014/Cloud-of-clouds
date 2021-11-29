using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chilkat;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Files;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;

namespace COC.Dropbox
{
    public class Uploader
    {
        public static void UploadFile(IFileSystemUnit fileSystemUnit, string fileToUploadPath, Dictionary<string, Dictionary<string, string>> mailToToken)
        {
            var splittedPath = fileSystemUnit.Path.Split('/');
            if (splittedPath.Length < 3)
            {
                Console.WriteLine("Upload in this folder is unsupported");
                return;
            }
            var service = splittedPath[2];
            var path = string.Join("/", splittedPath.Skip(3));
            if (splittedPath.Length > 3)
                path = '/' + path;
            var mail = fileSystemUnit.Mail;
            var token = mailToToken[mail][service];
            var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
            var fileName = fileToUploadPath.Split('\\').Last();
            if (isFile)
                Console.WriteLine("You can't upload file into file");
            else if (service == "yandex")
                YandexUploader(path + '/' + fileName, fileToUploadPath, token);
            else if (service == "dropbox")
                DropboxUploader(path + '/' + fileName, fileToUploadPath, token);
            else
                Console.WriteLine("Unknown service");
        }
        private static void DropboxUploader(string pathToUpload, string fileToUploadPath, string token)
        {
            var dropboxClient = new DropboxClient(token);
            var file = File.Open(fileToUploadPath, FileMode.Open);
            var _ = dropboxClient.Files.UploadAsync(pathToUpload, WriteMode.Add.Instance,
                body: file).Result;
        }
        
        private static void YandexUploader(string pathToUpload, string fileToUploadPath, string token)
        {
            var yandexClient = new DiskHttpApi(token);
            var file = File.Open(fileToUploadPath, FileMode.Open);
            Task.Run(() => yandexClient.Files.UploadFileAsync(pathToUpload, false, file));
        }
    }
}