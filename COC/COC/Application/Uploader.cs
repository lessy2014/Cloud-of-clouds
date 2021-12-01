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
    public static class Uploader
    {
        public static void UploadFile(IFileSystemUnit fileSystemUnit, string fileToUploadPath)
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
            var token = fileSystemUnit.Account.ServicesTokens[service];
            var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
            var fileName = fileToUploadPath.Split('\\').Last();
            if (isFile)
                Console.WriteLine("You can't upload file into file");
            else switch (service)
            {
                case "yandex":
                    YandexUploader.UploadFile(path + '/' + fileName, fileToUploadPath, token);
                    break;
                case "dropbox":
                    DropboxUploader.UploadFile(path + '/' + fileName, fileToUploadPath, token);
                    break;
                default:
                    Console.WriteLine("Unknown service");
                    break;
            }
        }
    }
}