using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Sharing;
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
            {
                var isFile = fileSystemUnit.GetType() == typeof(Infrastructure.File);
                Console.WriteLine(DropboxDownloader(path, token, isFile));
            }

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

        private static string DropboxDownloader(string path, string token, bool isFile)
        {
            var dropboxClient = new DropboxClient(token);
            if (isFile)
                if (!dropboxClient.Files.GetMetadataAsync(path).Result.AsFile.IsDownloadable)
                    return "This file is not downloadable";
            //var link = dropboxClient.Files.GetTemporaryLinkAsync(path).Result.Link;
            //cd sigmarblessme@gmail.com/dropbox/Folder1
            var link = "";
            var gotLink = false;
            var links = dropboxClient.Sharing.ListSharedLinksAsync(path).Result.Links;
            if (links.Count != 0)
            {
                foreach (var li in links)
                {
                    if (li.PathLower != path.ToLower())
                        continue;
                    gotLink = true;
                    link = li.Url;
                    break;
                }
            }
            if (!gotLink)
                link = dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync(path).Result.Url;
            var parts = link.Split(new [] {"dl=0"}, StringSplitOptions.None);
            return parts[0] + "dl=1" + parts[1];
        }
        
    }
}