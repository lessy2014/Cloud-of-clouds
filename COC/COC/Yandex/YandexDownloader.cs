using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using COC.Infrastructure;
using Dropbox.Api;
using Dropbox.Api.Sharing;
using YandexDisk.Client.Http;


namespace COC
{
    public static class YandexDownloader
    {
        public static string DownloadFile(string path, string token)
        {
            var yandexClient = new DiskHttpApi(token);
            var link = yandexClient.Files.GetDownloadLinkAsync(path).Result.Href;
            return link;
        }
    }
}