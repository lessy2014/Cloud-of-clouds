using System;
using System.Linq;
using COC.Dropbox;
using COC.Infrastructure;
using COC.Yandex;

namespace COC.Application
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
            switch (service)
            {
                case "yandex":
                    Console.WriteLine(YandexDownloader.DownloadFile(path, token));
                    break;
                case "dropbox":
                {
                    var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
                    Console.WriteLine(DropboxDownloader.DownloadFIle(path, token, isFile));
                    break;
                }
                default:
                    throw new ArgumentException("Unknown service");
            }
        }
    }
}