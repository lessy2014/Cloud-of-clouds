using System.IO;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using File = System.IO.File;
using Task = System.Threading.Tasks.Task;


namespace COC.Yandex
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