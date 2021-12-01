using YandexDisk.Client.Http;

namespace COC.Yandex
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