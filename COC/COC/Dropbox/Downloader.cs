using System;
using System.Collections.Generic;
using System.Linq;
using COC.Infrastructure;
using YandexDisk.Client.Http;

namespace COC.Dropbox
{
    public class Downloader
    {
        public static void DownloadFile(IFileSystemUnit fileSystemUnit, Dictionary<string, Dictionary<string, string>> mailToToken)
        {
            var path = "/" + string.Join("/", fileSystemUnit.Path.Split('/').Skip(3));
            if (path == "/")
            {
                Console.WriteLine("This folder is not downloadable");
                return;
            }
            var mail = fileSystemUnit.Mail;
            var token = mailToToken[mail]["yandex"];
            var yandexClient = new DiskHttpApi(token);
            // path = "/YandexFolder1/YandexPresentation1.pptx";
            var link = yandexClient.Files.GetDownloadLinkAsync(path).Result;
            Console.WriteLine(link.Href);
        }
    }
}