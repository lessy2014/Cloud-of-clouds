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


namespace COC
{
    public static class YandexUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, string token)
        {
            var yandexClient = new DiskHttpApi(token);
            var file = File.Open(fileToUploadPath, FileMode.Open);
            Task.Run(() => yandexClient.Files.UploadFileAsync(pathToUpload, false, file));
        }
    }
}