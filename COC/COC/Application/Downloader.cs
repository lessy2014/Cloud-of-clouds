using System;
using System.Linq;
using COC.Dropbox;
using COC.Infrastructure;
using COC.Yandex;

namespace COC.Application
{
    public static class Downloader
    {
        public static void DownloadFile(IFileSystemUnit fileSystemUnit, IDownloader downloader)
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
            var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
            Console.WriteLine(downloader.DownloadFile(path, token, isFile));
        }
    }
}