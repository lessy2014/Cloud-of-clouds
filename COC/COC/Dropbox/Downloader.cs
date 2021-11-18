using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Infrastructure;
using Dropbox.Api;
using YandexDisk.Client.Http;

namespace COC.Dropbox
{
    public class Downloader
    {
        public static void DownloadFile(IFileSystemUnit fileSystemUnit, Dictionary<string, Dictionary<string, string>> mailToToken)
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
            var token = mailToToken[mail][service];
            if (service == "yandex")
                Console.WriteLine(YandexDownloader(path, token));
            else
                Console.WriteLine(DropboxDownloader(path, token));
            Console.WriteLine();
            // var path = "/" + string.Join("/", fileSystemUnit.Path.Split('/').Skip(3));
            // if (path == "/")
            // {
            //     Console.WriteLine("This folder is not downloadable");
            //     return;
            // }
            // var yandexClient = new DiskHttpApi(token);
            // // path = "/YandexFolder1/YandexPresentation1.pptx";
            // var link = yandexClient.Files.GetDownloadLinkAsync(path).Result;
            // Console.WriteLine(link.Href);
        }

        private static string YandexDownloader(string path, string token)
        {
            var yandexClient = new DiskHttpApi(token);
            // path = "/YandexFolder1/YandexPresentation1.pptx";
            var link = yandexClient.Files.GetDownloadLinkAsync(path).Result.Href;
            return link;
        }

        private static string DropboxDownloader(string path, string token)
        {
            var dropboxClient = new DropboxClient(token);
            // path = "Folder1/Document1.gdoc";
            // path = dropboxClient.Files.ListFolderAsync(path).Result.Entries.ToList()[2].PathDisplay;
            // Console.WriteLine(path);
            var link = dropboxClient.Files.GetTemporaryLinkAsync(path).Result.Link;
            return link;
        }
    }
}