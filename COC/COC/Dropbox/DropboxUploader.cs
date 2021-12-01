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
    public static class DropboxUploader
    {
        public static void UploadFile(string pathToUpload, string fileToUploadPath, string token)
        {
            var dropboxClient = new DropboxClient(token);
            var file = File.Open(fileToUploadPath, FileMode.Open);
            var _ = dropboxClient.Files.UploadAsync(pathToUpload, WriteMode.Add.Instance,
                body: file).Result;
        }
    }
}