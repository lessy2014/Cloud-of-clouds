using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Application;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Sharing;
using YandexDisk.Client.Http;

namespace COC
{
    public static class Downloader
    {
        public static void DownloadFile(IFileSystemUnit fileSystemUnit)
        {
            var splittedPath = fileSystemUnit.Path.Split('/');
            if (splittedPath.Length < 4)
            {
                Console.WriteLine("This folder is not downloadable");
                return;
            }
            var service = splittedPath[2];
            var path = "/" + string.Join("/", splittedPath.Skip(3));
            var mail = fileSystemUnit.Mail;
            var token = fileSystemUnit.Account.ServicesTokens[service];
            if (service == "yandex")
                Console.WriteLine(YandexDownloader.DownloadFile(path, token));
            else
            {
                var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
                Console.WriteLine(DropboxDownloader.DownloadFIle(path, token, isFile));
            }
        }
    }
}